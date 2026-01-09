using UnityEngine;
using UnityEngine.UIElements;

public class PlayerJump : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator animator;

    [Header("Jump setting")]
    [SerializeField] private float jumpForce = 6;

    public PlayerGroundCheck GroundCheck;
    void Update()
    {
        if (Input.GetButtonDown("Jump") && GroundCheck.IsGrounded)// jumping
        {
            Jump();
        }

        UpdateAnimation();// up to state
    }
    private void Jump()
    {
        //reset velocity Y
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocityX, 0f);
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        animator.SetBool("isJump", true);
        animator.SetBool("isFalling", false);
        
    }
    private void UpdateAnimation()
    {
        //on the ground
        if (GroundCheck.IsGrounded)
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFalling", false);
            return;
        }

        if (rigidBody.linearVelocityY > 0.1f)// Jumping
        {
            animator.SetBool("isJump", true);
            animator.SetBool("isFalling", false);
        }
        else if (rigidBody.linearVelocityY < -0.1f)// falling
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFalling", true);
        }
    }
}
