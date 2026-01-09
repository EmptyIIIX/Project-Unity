using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Horizontal speed
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private Animator animator;

    private Vector2 movement;
    private float xPosLastFrame;// for collect last value of moveInput

    void Update()
    {
        MovementX();
        FlipCharacterX();
    }

    private void MovementX()
    {
        // 1) Read horizontal input (A/D or Left/Right arrows)
        float moveInput = Input.GetAxisRaw("Horizontal");
        movement.x = moveInput * speed * Time.deltaTime;
        transform.Translate(movement);

        // 2) check isRunning to animation
        if (moveInput != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
    private void FlipCharacterX()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0 && (transform.position.x > xPosLastFrame))
        {
            //player is moving right
            SpriteRenderer.flipX = false;
        }
        else if(moveInput < 0 && (transform.position.x < xPosLastFrame))
        {
            //player is moving left
            SpriteRenderer.flipX = true;
        }

        xPosLastFrame = transform.position.x;
    }
}
