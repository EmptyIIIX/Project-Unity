using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance;
    [SerializeField] private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPosition;
    public LayerMask EnemiesLayer;
    public float attackRange;

    public int NormalAttackDamage = 1;
    public bool isAttacking;

    public bool canReceiveInput;
    public bool inputReceived;

    public static event Action HitEnemy;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (timeBtwAttack > 0)
        {
            timeBtwAttack -= Time.deltaTime;
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            NormalAttack();
        }
    }
    public void InputManager()
    {
        if(!canReceiveInput)
        {
            canReceiveInput = true;
        }
        else
        {
            canReceiveInput = false;
        }
    }

    void NormalAttack()
    {
        if (canReceiveInput)
        {
            inputReceived = true;
            canReceiveInput = false;
        }
        else
        {
            return;
        }

        
        isAttacking = true;

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, EnemiesLayer);

        foreach (Collider2D enemy in enemiesToDamage)
        {
            //for class enemy
            enemy.GetComponent<EnemyGuardMovement>()?.TakeDamage(NormalAttackDamage);
            enemy.GetComponentInParent<HellHound_script>()?.TakeDamage(NormalAttackDamage);
            enemy.GetComponent<BossHealth>()?.TakeDamage(NormalAttackDamage);
            HitEnemy.Invoke();
        }

        timeBtwAttack = startTimeBtwAttack;
        StartCoroutine(StopAttack());
    }

    private IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(startTimeBtwAttack);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
