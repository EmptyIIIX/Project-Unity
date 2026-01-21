using System.Collections;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;

    [Header("Movement setting")]
    [SerializeField] private float speed = 5f; // Horizontal speed

    [Header("Dash Settings")]
    [SerializeField] private float dashDistance = 2;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float cooldownDash = 1f;
    [SerializeField] private LayerMask obstacleLayer;

    private float moveInput;
    private bool isDashing;
    private float dashCooldownTimer;

    void Update()
    {
        ReadInput();
        HandleDash();
        FlipCharacter();
        UpdateAnimation();
    }
    private void FixedUpdate()
    {
        if (!isDashing)
        {
            Movement();
        }
    }
    private void ReadInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
    }
    private void Movement()
    {
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocityY); ;
        //Vector3 movement = new Vector3(moveInput * speed * Time.deltaTime, 0f, 0f);
        //transform.Translate(movement);
    }
    private void  HandleDash()
    {
        if(dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetButtonDown("Fire2") && moveInput != 0 && !isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }
    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        dashCooldownTimer = cooldownDash;

        animator.SetTrigger("Dash");

        float elapsed = 0f;
        Vector2 dashDir = new Vector2(Mathf.Sign(moveInput), 0);

        while (elapsed < dashDuration)
        {
            float step = (dashDistance / dashDuration) * Time.fixedDeltaTime;

            RaycastHit2D hit = Physics2D.Raycast(rb.position, dashDir, step, obstacleLayer);

            if(hit.collider != null)
            {
                break;
            }

            rb.MovePosition(rb.position + dashDir * step);

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        isDashing = false;
    }
    private void FlipCharacter()
    {
        if (moveInput == 0) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(moveInput) * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
    private void UpdateAnimation()
    {
        animator.SetBool("isRunning", moveInput != 0);
    }
}
