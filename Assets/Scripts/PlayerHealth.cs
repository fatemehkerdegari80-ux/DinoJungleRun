
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public float damageCooldown = 1f;

    public int CurrentLives { get; private set; }

    private float nextDamageTime;
    private bool isDead;

    private void Awake()
    {
        CurrentLives = maxLives;
    }

    public void TakeDamage()
    {
        if (isDead || Time.time < nextDamageTime)
            return;

        CurrentLives--;
        nextDamageTime = Time.time + damageCooldown;

        Debug.Log("Lives remaining: " + CurrentLives);

        if (CurrentLives <= 0)
        {
            CurrentLives = 0;
            isDead = true;

            Debug.Log("GAME OVER!");
        }
    }
}
