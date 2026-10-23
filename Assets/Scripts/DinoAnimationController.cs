
using System.Collections;
using UnityEngine;

public class DinoAnimationController : MonoBehaviour
{
    [Header("Player References")]
    public PlayerDash playerDash;
    public PlayerJump playerJump;
    public PlayerHealth playerHealth;
    public GameOverMenu gameOverMenu;

    [Header("Death Settings")]
    public float deathAnimationDuration = 1.1f;
    public float deathFallDuration = 0.7f;
    public float deathGroundOffset = 0f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;

    private Color originalColor;
    private int lastLives;

    private Coroutine hitCoroutine;
    private bool deathStarted = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (animator != null)
        {
            animator.speed = 1f;
            animator.updateMode = AnimatorUpdateMode.Normal;
        }

        if (playerJump != null)
        {
            playerCollider =
                playerJump.GetComponent<Collider2D>();
        }

        if (playerHealth != null)
        {
            lastLives = playerHealth.CurrentLives;
        }
    }

    private void Update()
    {
        if (animator == null || playerHealth == null)
            return;

        int lives = playerHealth.CurrentLives;

        // Detect health changes
        if (lives != lastLives)
        {
            if (lives <= 0)
            {
                PlayDeath();
            }
            else if (lives < lastLives)
            {
                if (hitCoroutine != null)
                {
                    StopCoroutine(hitCoroutine);
                }

                hitCoroutine = StartCoroutine(PlayHit());
            }

            lastLives = lives;
        }

        if (deathStarted || lives <= 0)
            return;

        // Sprint animation
        if (playerDash != null)
        {
            bool isSprinting =
                playerDash.CurrentSpeed >
                playerDash.normalSpeed;

            animator.SetBool("IsSprinting", isSprinting);
        }

        // Jump animation
        if (playerCollider != null &&
            playerJump != null &&
            playerJump.groundCollider != null)
        {
            bool isGrounded =
                playerCollider.IsTouching(
                    playerJump.groundCollider
                );

            animator.SetBool("IsJumping", !isGrounded);
        }
    }

    // =========================
    // HIT ANIMATION
    // =========================

    private IEnumerator PlayHit()
    {
        if (deathStarted)
            yield break;

        animator.SetBool("IsHit", true);
        animator.SetTrigger("Hit");

        if (spriteRenderer != null)
        {
            spriteRenderer.color =
                new Color32(255, 120, 120, 255);
        }

        yield return new WaitForSeconds(0.15f);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        yield return new WaitForSeconds(0.10f);

        animator.SetBool("IsHit", false);

        hitCoroutine = null;
    }

    // =========================
    // DEATH ANIMATION
    // =========================

    private void PlayDeath()
    {
        if (deathStarted)
            return;

        deathStarted = true;

        // Cancel Hit animation
        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
            hitCoroutine = null;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        // Reset other states
        animator.ResetTrigger("Hit");
        animator.SetBool("IsHit", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsDead", true);

        // Keep Death animation running while world is frozen
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        animator.speed = 1f;

        animator.Play("Base Layer.Dino_Death", 0, 0f);

        Debug.Log("Death animation started!");

        // Stop world movement immediately
        Time.timeScale = 0f;

        // Let dinosaur land, finish death animation,
        // and then show Game Over
        StartCoroutine(FinishDeath());
    }

    // =========================
    // FALL TO GROUND + GAME OVER
    // =========================

    private IEnumerator FinishDeath()
    {
        Transform playerRoot = null;
        float dropDistance = 0f;

        if (playerJump != null)
        {
            playerRoot = playerJump.transform;
        }

        // Calculate distance to physical ground
        if (playerRoot != null &&
            playerCollider != null &&
            playerJump.groundCollider != null)
        {
            float groundTop =
                playerJump.groundCollider.bounds.max.y;

            float playerBottom =
                playerCollider.bounds.min.y;

            dropDistance = Mathf.Max(
                0f,
                playerBottom - groundTop
            );
        }

        Vector3 startPosition =
            playerRoot != null
                ? playerRoot.position
                : Vector3.zero;

        Vector3 targetPosition =
            startPosition +
            Vector3.down * dropDistance;

        targetPosition.y += deathGroundOffset;

        float elapsed = 0f;

        float totalDuration =
            Mathf.Max(0.1f, deathAnimationDuration);

        float fallDuration =
            Mathf.Max(0.05f, deathFallDuration);

        // Run independently of Time.timeScale
        while (elapsed < totalDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            if (playerRoot != null)
            {
                float t = Mathf.Clamp01(
                    elapsed / fallDuration
                );

                // Smooth landing
                t = t * t * (3f - 2f * t);

                playerRoot.position = Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );
            }

            yield return null;
        }

        // Ensure exact final position
        if (playerRoot != null)
        {
            playerRoot.position = targetPosition;
        }

        // Freeze the final Death frame
        animator.speed = 0f;

        // Show Game Over menu
        if (gameOverMenu != null)
        {
            gameOverMenu.ShowGameOver();
        }
        else
        {
            Debug.LogError(
                "GameOverMenu is not assigned!"
            );
        }

        Debug.Log("GAME OVER!");
    }
}
