
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    public float speedMultiplier = 1f;

    private PlayerDash playerDash;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerDash = FindAnyObjectByType<PlayerDash>();        
        mainCamera = Camera.main;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (playerDash == null || mainCamera == null)
        {
            Debug.LogError("ScrollingObject: PlayerDash or Main Camera not found!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (playerDash == null || mainCamera == null)
            return;

        // Move left using the same speed as the ground
        float movement = playerDash.CurrentSpeed
                         * speedMultiplier
                         * Time.deltaTime;

        transform.position += Vector3.left * movement;

        // Destroy after leaving the left side of the screen
        float cameraLeft =
            mainCamera.transform.position.x
            - mainCamera.orthographicSize * mainCamera.aspect;

        if (spriteRenderer != null &&
            spriteRenderer.bounds.max.x < cameraLeft - 1f)
        {
            Destroy(gameObject);
        }
    }
}
