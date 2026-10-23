
using UnityEngine;

public class GroundObstacle : Interactable
{
    public PlayerDash playerDash;

    public float destroyX = -11f;

    private void Update()
    {
        if (playerDash == null)
            return;

        float movement =
            playerDash.CurrentSpeed * Time.deltaTime;

        transform.position += Vector3.left * movement;

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnPlayerInteract(GameObject player)
    {
        Debug.Log("Player hit the log obstacle!");
    }
}
