using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private EnemyDetector detector;

    [Header("Enemy states")]
    public int health = 3;
    public float speed = 2f;
    public float knockbackForce = 5f;

    private bool isKnockback;
    void Start()
    {
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        if (health <= 0)
        {
            Die();
            return;
        }

        if (detector.isPlayerDetected)
        {
            FacePlayer();
            ChasePlayer();
        }
    }
    private void ChasePlayer()
    {
        if (isKnockback) return;

        //direction to face player
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocityY);
    }
    private void FacePlayer()
    {
        float diffX = player.position.x - transform.position.x;

        //fix direction to right when player and the enemy are same position x
        if(Mathf.Abs(diffX) < 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
            return;
        }

        //look at player
        transform.localScale = new Vector3(Mathf.Sign(diffX), 1, 1);
    }
    public void TakeDamage(int damage)
    {
        //Instantiate(bloodEffect, transform.position, Quaternion.identity);
        health -= damage;
        //animator.SetTrigger("Hit");

        Knockback();
        Debug.Log("damage TAKEN");
    }
    private void Knockback()
    {
        isKnockback = true;

        //direction to knock
        float knockDir = Mathf.Sign(transform.position.x - player.position.x);

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(
            new Vector2(knockDir * knockbackForce, 2f),
            ForceMode2D.Impulse
        );

        //cancel knockback
        Invoke(nameof(ResetKnockback), 0.2f);
    }
    private void ResetKnockback()
    {
        isKnockback = false;
    }
    private void Die()
    {
        //animator.SetTrigger("Die");
        Destroy(gameObject, 0.5f);
    }
}
