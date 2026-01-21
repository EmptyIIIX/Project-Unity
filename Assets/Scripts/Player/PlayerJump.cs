using UnityEngine;
using UnityEngine.UIElements;

public class PlayerJump : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator animator;
    public PlayerGroundCheck GroundCheck;

    [Header("Jump setting")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jumpCutMultiplier = 2.5f;
    [SerializeField] private float fallMultiplier = 3f;

    [Header("Wall Cling / Slide")]
    [SerializeField] private float timeClingLimit = 3f;     // เวลาที่เกาะกำแพงได้
    [SerializeField] private float wallSlideSpeed = -1.5f;  // ความเร็วไถลลง

    private float durationCling;
    private bool isHoldingJump;
    private bool isWallSliding;

    void Start()
    {
        durationCling = timeClingLimit;
    }

    void Update()
    {
        HandleCling();
        HandleWallSlide();
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

    private void HandleCling()
    {
        // assign physic material
        if (GroundCheck.IsClingable)
        {
            rigidBody.sharedMaterial = null;
        }
        else
        {
            rigidBody.sharedMaterial = GroundCheck.PhyMat;
        }

        // clinging
        if (Input.GetButtonDown("Jump") && GroundCheck.IsClingable && !GroundCheck.IsGrounded)
        {
            ClingJump();
        }

        DurationTimeCling();
    }

    private void ClingJump()
    {
        // reset velocity Y
        rigidBody.linearVelocity = new Vector2(
            rigidBody.linearVelocityX,
            0f
        );

        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        isHoldingJump = true;
        isWallSliding = false;

        animator.SetBool("isJump", true);
        animator.SetBool("isFalling", false);
        animator.SetBool("isCling", false);
    }

    private void DurationTimeCling()
    {
        // ระหว่างเกาะกำแพง
        if (GroundCheck.IsClingable && !GroundCheck.IsGrounded)
        {
            // เฉพาะตอนกำลังจะตก
            if (rigidBody.linearVelocityY <= 0f)
            {
                durationCling -= Time.deltaTime;

                // หมดเวลา = เริ่มไถล
                if (durationCling <= 0f)
                {
                    isWallSliding = true;
                }
            }
        }
        else
        {
            // แตะพื้น หรือ หลุดกำแพง
            durationCling = timeClingLimit;
            isWallSliding = false;
        }
    }

    private void HandleWallSlide()
    {
        // ถ้าเกาะกำแพง และ cling หมดเวลา = ไถลลง
        if (isWallSliding && GroundCheck.IsClingable && !GroundCheck.IsGrounded)
        {
            rigidBody.linearVelocity = new Vector2(
                rigidBody.linearVelocityX,
                wallSlideSpeed
            );

            // ปิด gravity เพื่อไม่ให้ตกแรงเกิน
            rigidBody.gravityScale = 0f;

            rigidBody.sharedMaterial = GroundCheck.PhyMat;
        }
    }

    private void Jump()
    {
        // reset velocity Y
        rigidBody.linearVelocity = new Vector2(
            rigidBody.linearVelocityX,
            0f
        );

        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        isHoldingJump = true;

        animator.SetBool("isJump", true);
        animator.SetBool("isFalling", false);
    }

    private void ApplyBetterJumpPhysics()
    {
        // ถ้า wall slide อยู่ ไม่ใช้ better jump
        if (isWallSliding) return;

        if (rigidBody.linearVelocityY < 0f) // falling
        {
            rigidBody.gravityScale = fallMultiplier;
        }
        else if (rigidBody.linearVelocityY > 0f && !isHoldingJump) // jump cut
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
        // on the ground
        if (GroundCheck.IsGrounded)
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isCling", false);
            return;
        }

        // cling wall (duration on time)
        if (GroundCheck.IsClingable && !GroundCheck.IsGrounded)
        {
            animator.SetBool("isCling", true);
        }
        else
        {
            animator.SetBool("isCling", false);
        }

        // jump / fall
        if (rigidBody.linearVelocityY > 0.1f)
        {
            animator.SetBool("isJump", true);
            animator.SetBool("isFalling", false);
        }
        else if (rigidBody.linearVelocityY < -0.1f)
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isFalling", true);
        }
    }
}
