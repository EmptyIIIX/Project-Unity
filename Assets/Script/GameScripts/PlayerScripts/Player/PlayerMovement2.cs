using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovement2 : MonoBehaviour
{

    enum PlayerStateMovement
    {
        Idle,
        Run,
        Jump,
        Wall
    }
    PlayerStateMovement mMovement;

    [Header("Reference")]
    public Rigidbody2D rb;
    public Animator animator;
    public PlayerHealth2 playerHealth;

    [Header("Movement")]
    public float moveSpeed = 5;
    public float horizontalMovement;
    public bool iscanMove = true;

    [Header("Dashing")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 0.1f;
    bool isDashing;
    bool canDash = true;

    [Header("Jumping")]
    public float jumpForce = 5;
    public int maxJumps = 1;
    int jumpRemaining;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;

    [Header("WallMovement")]
    public float wallSlideSpeed = 0.5f;
    public float wallJumpTime = 0.25f;
    public Vector2 wallJumpForce = new Vector2(5f, 5f);
    bool isWallSliding;

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.05f, 0.5f);
    public LayerMask wallLayer;

    [Header("Gravity")]
    public float baseGravity = 1f;
    public float maxFallSpeed = 4f;
    public float fallSpeedMultiplier = 1f;

    bool isFacingRight = false;

    //wall jumping
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTimer;

    // Update is called once per frame
    void Update()
    {
        if (!iscanMove) { return; }
        else
        {
            if (isDashing) { return; }
            if (PlayerAttack.Instance.isAttacking)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                return;
            }
            PlayerInput();
            GroundCheck();
            Gravity();
            WallSlide();
            WallJump();
            if (!isWallJumping)
            {
                MoveHorizon();
                Flip();
            }
            UpdateAnim();
        }
    }
    private void PlayerInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
    }

    private void Flip()
    {
        if(isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }

    private void Gravity()
    {
        if(rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void MoveHorizon()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if(context.performed && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        Physics2D.IgnoreLayerCollision(20, 9, true);
        canDash = false;
        isDashing = true;
        playerHealth.isImmune = true;
        animator.SetBool("isDashing?", true);

        float dashDirection = isFacingRight ? 1f : -1f;

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        
        isDashing = false;
        Physics2D.IgnoreLayerCollision(20, 9, false);

        yield return new WaitForSeconds(dashCooldown);
        
        playerHealth.isImmune = false;
        canDash = true;
    }

    private void GroundCheck()
    {
        if(Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpRemaining = maxJumps;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //jump on ground
        if (jumpRemaining > 0)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpRemaining--;
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocityY * 0.5f);
                jumpRemaining--;
            }
        }
        
        //wall jump
        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(-wallJumpDirection * wallJumpForce.x, wallJumpForce.y);
            wallJumpTimer = 0;

            //force flip
            if(transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1;
                transform.localScale = ls;
            }

            Invoke(nameof(CancleWallJump), wallJumpTime + 0.1f);
        }
    }

    private void WallSlide()
    {
        if (!isGrounded & WallCheck() & horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancleWallJump));
        }
        else if(wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancleWallJump()
    {
        isWallJumping = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }

    private void UpdateAnim()
    {
        //running
        if (rb.linearVelocity.x != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        //dashing
        if(isDashing)
        {
            animator.SetBool("isDashing?", true);
        }
        else
        {
            animator.SetBool("isDashing?", false);
        }

        //jumping
        if (rb.linearVelocity.y != 0)
        {
            animator.SetBool("isJump", true);
        }
        else
        {
            animator.SetBool("isJump", false);
        }

        //clinging
        if(isWallSliding)
        {
            animator.SetBool("isClinging", true );
        }
        else
        {
            animator.SetBool("isClinging", false );
        }
    }
}
