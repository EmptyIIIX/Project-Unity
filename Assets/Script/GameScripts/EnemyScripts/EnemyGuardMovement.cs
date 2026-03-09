using UnityEngine;

public class EnemyGuardMovement : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    public int damage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //is ground?
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        //player direction
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        if (isGrounded)
        {
            //chase player
            rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);
        }
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            Vector2 direction = (player.position - transform.position).normalized;
        }
    }
}
