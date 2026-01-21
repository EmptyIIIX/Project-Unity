using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Collision controller")]
    [SerializeField] private float bounceForce = 6f;

    private float halfHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        halfHeight = spriteRenderer.bounds.extents.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            CollideWithEnemy(collision);
        }
    }
    private void CollideWithEnemy(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (Physics2D.Raycast(transform.position, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Enemy")))
        {
            // hit enemy top (step on enemy's head)
            Vector2 velocity = rigidBody.linearVelocity;
            velocity.y = 0f;
            rigidBody.linearVelocity = velocity;
            rigidBody.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            enemy.Die();
        }
        else
        {
            enemy.HitPlayer(transform);
        }
    }
}
