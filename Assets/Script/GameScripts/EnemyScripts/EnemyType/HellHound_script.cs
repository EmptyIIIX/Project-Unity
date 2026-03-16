using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class HellHound_script : MonoBehaviour, IEnemy
{
    public Transform leftPoint;
    public Transform rightPoint;
    public Animator animator;
    SpriteRenderer spriteRenderer;
    public Rigidbody2D hellhoundRB;
    AudioManager audioManager;
    private Color ogColor;

    public int currentHealth;
    public int maxHealth = 3;
    private bool isDie = false;
    public float moveSpeed = 3f;
    public float waitToChangePoint = 2f;

    Transform targetPoint;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        targetPoint = (Random.value < 0.5f) ? leftPoint : rightPoint;
        spriteRenderer = hellhoundRB.GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        ogColor = spriteRenderer.color;
        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            float dir = targetPoint.position.x - transform.position.x;

            if (dir > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);

            while (Vector2.Distance(transform.position, targetPoint.position) > 0.1f && isDie == false)
            {
                animator.SetBool("isRunning", true);
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    targetPoint.position,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }

            animator.SetBool("isRunning", false);
            yield return new WaitForSeconds(waitToChangePoint);

            targetPoint = (targetPoint == leftPoint) ? rightPoint : leftPoint;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDie) { return; }
        currentHealth -= damage;
        StartCoroutine(FlashWhite());
        if (currentHealth <= 0)
        {
            //animator.SetTrigger("Die");
            isDie = true;
            StartCoroutine(animDie());
        }
    }
    private IEnumerator animDie()
    {
        animator.SetTrigger("die");

        yield return new WaitForSeconds(1f);

        audioManager.PlayerSFX(audioManager.PlayerDead);

        Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator FlashWhite()
    {
        ogColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = ogColor;

    }

}
