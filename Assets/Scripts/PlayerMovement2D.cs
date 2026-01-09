using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Movement setting")]
    [SerializeField] private float speed = 5f; // Horizontal speed

    private float moveInput;

    void Update()
    {
        ReadInput();
        Movement();
        FlipCharacter();
        UpdateAnimation();
    }
    private void ReadInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
    }
    private void Movement()
    {
        Vector3 movement = new Vector3(moveInput * speed * Time.deltaTime, 0f, 0f);
        transform.Translate(movement);
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
