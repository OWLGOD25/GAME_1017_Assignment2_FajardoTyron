using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float speed = 0.5f; // smaller = slower scroll
    public float tileWidth = 13f; // width in world units of one tile

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float displacement = Mathf.Repeat(Time.time * speed, tileWidth);
        transform.position = startPos + Vector3.left * displacement;
    }
}
