
using UnityEngine;

public class GroundScroller : MonoBehaviour
{
    public PlayerDash playerDash;

    private Transform[] tiles;
    private float tileWidth;
    private Camera gameCamera;

    private void Start()
    {
        if (transform.childCount != 3)
        {
            Debug.LogError("GroundVisuals needs exactly 3 tiles.");
            enabled = false;
            return;
        }

        tiles = new Transform[3];

        for (int i = 0; i < 3; i++)
        {
            tiles[i] = transform.GetChild(i);
        }

        SpriteRenderer sr =
            tiles[1].GetComponent<SpriteRenderer>();

        if (sr == null || sr.sprite == null)
        {
            Debug.LogError("GroundTile_2 needs a sprite!");
            enabled = false;
            return;
        }

        tileWidth = sr.bounds.size.x;

        // Position all three tiles next to each other
        float centerX = tiles[1].position.x;

        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = tiles[i].position;
            pos.x = centerX + (i - 1) * tileWidth;
            tiles[i].position = pos;
        }

        gameCamera = Camera.main;

        if (gameCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (playerDash == null)
            return;

        float movement =
            playerDash.CurrentSpeed * Time.deltaTime;

        // Move ground to the left
        foreach (Transform tile in tiles)
        {
            tile.position += Vector3.left * movement;
        }

        // Calculate left edge of camera
        float cameraLeft =
            gameCamera.transform.position.x -
            gameCamera.orthographicSize * gameCamera.aspect;

        // Recycle tiles after leaving the screen
        foreach (Transform tile in tiles)
        {
            SpriteRenderer sr =
                tile.GetComponent<SpriteRenderer>();

            if (sr.bounds.max.x < cameraLeft)
            {
                float rightmostX = Mathf.Max(
                    tiles[0].position.x,
                    tiles[1].position.x,
                    tiles[2].position.x
                );

                Vector3 pos = tile.position;
                pos.x = rightmostX + tileWidth;
                tile.position = pos;
            }
        }
    }
}
