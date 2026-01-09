using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public Animator animator;
    public float attackRange;
    public int damage;
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
            // enemy.GetComponent<Enemy>()?.TakeDamage(damage);
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
