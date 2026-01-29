using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Enemy movement")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private int startDirection = 1;
    [SerializeField] private bool stayOnLedges = true;

    private int currentDirection;
    private float halfWidth;
    private float halfHeight;
    private Vector2 movement;
    private bool isGrounded;
    private float movementDelay;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        halfWidth = spriteRenderer.bounds.extents.x;
        halfHeight = spriteRenderer.bounds.extents.y;
        currentDirection = startDirection;

        spriteRenderer.flipX = startDirection == 1 ? false : true;
    }

    private void FixedUpdate()
    {
        if(movementDelay > 0f)
        {
            movementDelay -= Time.fixedDeltaTime;
            return;
        }

        movement.x = speed * currentDirection;
        movement.y = rigidBody.linearVelocityY;
        rigidBody.linearVelocity = movement;

        SetDirection();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded= false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
    public void KnockbackEnemy(Vector2 knockbackForce, int direction, float delay)
    {
        movementDelay = delay;
        knockbackForce.x *= direction;
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        rigidBody.AddForce(knockbackForce, ForceMode2D.Impulse);
    }
    private void SetDirection()
    {
        if (!isGrounded) return;

        Vector2 rightPos = transform.position;
        Vector2 leftPos = transform.position;
        rightPos.x += halfWidth;
        leftPos.x -= halfWidth;

        if(rigidBody.linearVelocityX > 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.right, halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                currentDirection *= -1; // switch direction right-left
                spriteRenderer.flipX = true;
            }
            else if(stayOnLedges && !Physics2D.Raycast(rightPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground")))
            {
                currentDirection *= -1; // switch direction right-left
                spriteRenderer.flipX = true;
            }
        }
        else if(rigidBody.linearVelocityX < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.left, halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
                currentDirection *= -1; // switch direction left-right
                spriteRenderer.flipX = false;
            }
            else if (stayOnLedges && !Physics2D.Raycast(leftPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground")))
            {
                currentDirection *= -1; // switch direction right-left
                spriteRenderer.flipX = false;
            }
        }
    }
}
