using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float timeBtwAttack;
    [SerializeField] private SpriteRenderer playerSr;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float bounceForce = 6f;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public Animator animator;
    public float attackRange;
    public int damage;

    private float halfHeight;

    private void Start()
    {
        halfHeight = playerSr.bounds.extents.y;
    }
    void Update()
    {
        if(timeBtwAttack > 0)
        {
            timeBtwAttack -= Time.deltaTime;
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }
    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);

        foreach(Collider2D enemy in enemiesToDamage)
        {
            //for class enemy
             enemy.GetComponent<EnemyBase>()?.TakeDamage(damage);
        }

        timeBtwAttack = startTimeBtwAttack;
    }

    //draw attack range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
