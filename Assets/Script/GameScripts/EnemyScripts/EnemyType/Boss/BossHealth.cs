using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour, IEnemy
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    public int maxHealth = 30;
    public int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color ogColor;

    public bool isDie;

    BossStateAnimation stateAnimation;

    void Start()
    {
        currentHealth = maxHealth;
        stateAnimation = GetComponent<BossStateAnimation>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        ogColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        if (isDie) return;

        currentHealth -= damage;
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDie = true;

            StartCoroutine(Die());
        }
    }

    private IEnumerator FlashRed()
    {
        ogColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = ogColor;

    }

    IEnumerator Die()
    {
        stateAnimation.ChangeState(BossStateAnimation.BossState.Dead);
        yield return null;

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("boss_die") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f
        );

        Destroy(gameObject);
    }
}