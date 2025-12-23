using UnityEngine;

public class PlayerJumpTest : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerGroundCheck groundCheck;
    public float jumpForce = 5f;
    void Update()
    {
        // Allow jumping only when grounded
        if (Input.GetKeyDown(KeyCode.Space) && groundCheck.IsGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
