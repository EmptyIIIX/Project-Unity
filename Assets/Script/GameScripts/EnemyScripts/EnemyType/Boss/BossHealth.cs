using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour, IEnemy
{
    public int maxHealth = 30;
    public int currentHealth;

    public bool isDie;

    BossStateAnimation stateAnimation;

    void Start()
    {
        currentHealth = maxHealth;
        stateAnimation = GetComponent<BossStateAnimation>();
    }

    public void TakeDamage(int damage)
    {
        if (isDie) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDie = true;

            stateAnimation.ChangeState(BossStateAnimation.BossState.Dead);

            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}