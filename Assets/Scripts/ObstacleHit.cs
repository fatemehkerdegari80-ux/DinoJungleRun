
using UnityEngine;

public class ObstacleHit : Interactable
{
    private bool alreadyHit = false;

    protected override void OnPlayerInteract(GameObject player)
    {
        Debug.Log("Obstacle touched Player!");

        PlayerHealth health = player.GetComponent<PlayerHealth>();

        if (health == null)
        {
            Debug.LogError("PlayerHealth is missing on Player!");
            return;
        }

        if (alreadyHit)
            return;

        alreadyHit = true;

        health.TakeDamage();
    }
}
