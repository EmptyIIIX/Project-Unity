using UnityEngine;
using UnityEngine.UIElements;

public class PlayerJump : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator animator;
    public PlayerGroundCheck GroundCheck;

    [Header("Jump setting")]
    [SerializeField] private float jumpForce = 7;
    [SerializeField] private float jumpCutMultiplier = 2.5f;
    [SerializeField] private float fallMultiplier = 3f;

    private bool isHoldingJump;

    void Update()
    {
        HandleJumpInput();
        ApplyBetterJumpPhysics();
        UpdateAnimation();
    }
    private void HandleJumpInput()
    {
        // start jumping
        if (Input.GetButtonDown("Jump") && GroundCheck.IsGrounded)
        {
            Jump();
        }

        // short jump
        if (Input.GetButtonUp("Jump"))
        {
            isHoldingJump = false;
        }
    }
    private void Jump()
    {
        //reset velocity Y
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocityX, 0f);
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        isHoldingJump = true;

        animator.SetBool("isJump", true);
        animator.SetBool("isFalling", false);
        
    }
    private void ApplyBetterJumpPhysics()
    {
        if(rigidBody.linearVelocityY < 0f)//falling
        {
            rigidBody.gravityScale = fallMultiplier;
        }
        else if(rigidBody.linearVelocityY > 0f && !isHoldingJump)
        {
            rigidBody.gravityScale = jumpCutMultiplier;
        }
        else
        {
            rigidBody.gravityScale = 1f;
        }
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
