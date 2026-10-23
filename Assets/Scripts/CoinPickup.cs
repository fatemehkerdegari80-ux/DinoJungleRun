
using UnityEngine;

public class CoinPickup : Interactable
{
    private ScoreManager scoreManager;
    private bool collected;

    private void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
    }

    protected override void OnPlayerInteract(GameObject player)
    {
        if (collected)
            return;

        PlayerHealth health =
            player.GetComponentInParent<PlayerHealth>();

        if (health == null || health.CurrentLives <= 0)
            return;

        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found!");
            return;
        }

        collected = true;

        scoreManager.AddCoin();

        Destroy(gameObject);
    }
}
