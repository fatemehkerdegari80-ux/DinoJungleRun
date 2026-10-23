
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public float jumpSpeed = 10f;
    public Collider2D groundCollider;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private bool jumpPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    // Called by Player Input when Jump is pressed
    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpPressed = true;
        }
    }

    private void FixedUpdate()
    {
        if (jumpPressed &&
            groundCollider != null &&
            playerCollider.IsTouching(groundCollider))
        {
            rb.linearVelocity =
                new Vector2(rb.linearVelocity.x, jumpSpeed);
        }

        jumpPressed = false;
    }
}
