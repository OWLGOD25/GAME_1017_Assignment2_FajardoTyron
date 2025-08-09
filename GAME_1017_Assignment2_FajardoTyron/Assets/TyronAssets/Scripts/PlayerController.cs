using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float runSpeed = 3f;      // base forward auto-run speed (level scroll moves background)
    public float moveSpeed = 2f;     // player lateral control (A/D)
    public float jumpForce = 8f;
    public int maxLives = 3;

    [Header("Slide")]
    public float slideDuration = 0.6f;
    public Vector2 colliderNormalSize = new Vector2(1f, 1.8f);
    public Vector2 colliderSlideSize = new Vector2(1f, 0.9f);

    [Header("Invulnerability")]
    public float invulnTime = 10f;
    public float invulnBlinkInterval = 0.12f;

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;
    private SpriteRenderer sr;

    private bool isGrounded = false;
    private bool isSliding = false;
    private bool isInvulnerable = false;
    private int lives;
    private float gravityScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        lives = maxLives;
        gravityScale = rb.gravityScale;
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimator();
    }

    private void HandleInput()
    {
        // Horizontal control A/D
        float hor = 0f;
        if (Input.GetKey(KeyCode.A)) hor = -1f;
        if (Input.GetKey(KeyCode.D)) hor = 1f;
        Vector2 vel = rb.linearVelocity;
        vel.x = hor * moveSpeed;
        rb.linearVelocity = new Vector2(vel.x, rb.linearVelocity.y);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            MainController.Instance.SoundManager?.PlayJump();
        }

        // Slide / duck / roll
        if (Input.GetKeyDown(KeyCode.S) && !isSliding && IsGrounded())
        {
            StartCoroutine(DoSlide());
        }
    }

    private IEnumerator DoSlide()
    {
        isSliding = true;
        MainController.Instance.SoundManager?.PlayRoll();
        // shrink collider - assumes BoxCollider2D
        var box = col as BoxCollider2D;
        if (box != null)
        {
            box.size = colliderSlideSize;
            box.offset = new Vector2(box.offset.x, (colliderSlideSize.y - colliderNormalSize.y) / 2f);
        }

        yield return new WaitForSeconds(slideDuration);

        if (box != null)
        {
            box.size = colliderNormalSize;
            box.offset = Vector2.zero;
        }
        isSliding = false;
    }

    private bool IsGrounded()
    {
        // Simple check: velocity.y near 0 and transform near ground. For robust use Raycast.
        return Mathf.Abs(rb.linearVelocity.y) < 0.01f;
    }

    private void UpdateAnimator()
    {
        if (!anim) return;
        anim.SetBool("isGrounded", IsGrounded());
        anim.SetBool("isSliding", isSliding);
        anim.SetFloat("xSpeed", Mathf.Abs(rb.linearVelocity.x));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard") && !isInvulnerable)
        {
            TakeHit();
            // optionally disable the hazard if needed
        }
    }

    private void TakeHit()
    {
        lives--;
        MainController.Instance.SoundManager?.PlayHit();
        StartCoroutine(DoInvulnerability(invulnTime));
        GameManager.Instance.OnPlayerHit(lives);

        if (lives <= 0)
        {
            Die();
        }
    }

    private IEnumerator DoInvulnerability(float seconds)
    {
        isInvulnerable = true;

        // allow only ground collisions with obstacles: set hazard colliders to trigger by tag handling (obstacle manager)
        float end = Time.time + seconds;
        while (Time.time < end)
        {
            // blinking visual
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(invulnBlinkInterval);
        }
        sr.enabled = true;
        isInvulnerable = false;
    }

    private void Die()
    {
        // stop movement, play death animation and notify GameManager
        rb.linearVelocity = Vector2.zero;
        if (anim) anim.SetTrigger("Die");
        GameManager.Instance.OnPlayerDeath();
    }

    // public helper to set spawn/reset
    public void ResetPlayer()
    {
        lives = maxLives;
        // reset transform/anim etc.
        isInvulnerable = false;
        sr.enabled = true;
    }

    public int GetLives() => lives;
}
