using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public int startLives = 3;
    public float preGameDelay = 0.5f; // small wait before running

    [Header("References")]
    public PlayerController player;
    public UIManager uiManager;

    private float runTimer = 0f;
    private bool running = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        if (uiManager == null) uiManager = FindFirstObjectByType<UIManager>();
        if (player == null) player = FindFirstObjectByType<PlayerController>();

        // Start game
        StartCoroutine(BeginAfterDelay());
    }

    private System.Collections.IEnumerator BeginAfterDelay()
    {
        yield return new WaitForSeconds(preGameDelay);
        running = true;
        // play game music
        MainController.Instance.SoundManager?.PlayGameMusic();
    }

    private void Update()
    {
        if (!running) return;
        runTimer += Time.deltaTime;
        uiManager?.UpdateTimer(runTimer);
    }

    public void OnPlayerHit(int livesLeft)
    {
        uiManager?.UpdateLives(livesLeft);
        // make hazards triggers only or similar -- you can use tags to toggle colliders here
    }

    public void OnPlayerDeath()
    {
        running = false;
        // stop parallax/obstacles by disabling their scripts or using a global flag
        MainController.Instance.SoundManager?.PlayGameOverMusic();

        // Best time check
        float best = PlayerPrefs.GetFloat("BestTime", 0f);
        if (runTimer > best)
        {
            PlayerPrefs.SetFloat("BestTime", runTimer);
            best = runTimer;
        }
        uiManager?.UpdateBestTime(best);

        // wait for death animation then change to GameOver
        Invoke("LoadGameOver", 2.0f);
    }

    private void LoadGameOver()
    {
        // Save best time in PlayerPrefs so GameOver scene can display it
        PlayerPrefs.SetFloat("LastRunTime", runTimer);
        SceneManager.LoadScene("GameOver");
    }

    // Reset used if restarting the Game scene
    //find out if this is something I need to use in the GameOver scene
    //if so then go into uiManager and implement added reset button code
    public void ResetGame()
    {
        runTimer = 0f;
        uiManager?.UpdateTimer(0f);
        if (player != null) player.ResetPlayer();
        // re-enable systems etc.
    }
}
