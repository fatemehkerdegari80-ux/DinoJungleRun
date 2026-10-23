
using UnityEngine;

public class ParallaxLoop : MonoBehaviour
{
    public Transform tileA;
    public Transform tileB;

    public PlayerDash playerDash;

    public float scrollSpeed = 0.3f;

    private Vector3 startPosition;
    private float tileWidth;
    private float distance;

    void Start()
    {
        if (tileA == null || tileB == null)
        {
            Debug.LogError("Tiles are not assigned!");
            enabled = false;
            return;
        }

        SpriteRenderer sr = tileA.GetComponent<SpriteRenderer>();

        if (sr == null || sr.sprite == null)
        {
            Debug.LogError("TileA needs a Sprite Renderer!");
            enabled = false;
            return;
        }

        tileWidth = sr.sprite.bounds.size.x *
                    Mathf.Abs(tileA.localScale.x);

        startPosition = tileA.localPosition;

        tileB.localPosition = startPosition +
                              Vector3.right * tileWidth;
    }

    void Update()
    {
        float speedMultiplier = 1f;

        if (playerDash != null && playerDash.normalSpeed > 0f)
        {
            speedMultiplier =
                playerDash.CurrentSpeed / playerDash.normalSpeed;
        }

        distance = Mathf.Repeat(
            distance + scrollSpeed * speedMultiplier * Time.deltaTime,
            tileWidth
        );

        tileA.localPosition = startPosition +
                              Vector3.left * distance;

        tileB.localPosition = startPosition +
                              Vector3.right * (tileWidth - distance);
    }
}
