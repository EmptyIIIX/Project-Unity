using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth2 : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    public HealthUI healthUI;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement2 movement2;
    AudioManager audioManager;
    public static event Action OnPlayerDied;

    //Iframe player
    public bool isImmune;
    public float immuneTime = 1f;
    public bool isPlayerDie = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ResetHealth();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement2 = GetComponent<PlayerMovement2>();
        GameController.OnReset += ResetHealth;
        HoldToLoadLevel.OnHoldComplete += Heal;
        healthUI.SetMaxHearts(maxHealth);
        healthUI.UpdateHearts(currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //EnemyGuardMovement enemyGuard = collision.GetComponent<EnemyGuardMovement>();
        EnemyAttackHitbox attackHitbox = collision.GetComponent<EnemyAttackHitbox>();
        //if (enemyGuard)
        //{
        //    TakeDamage(enemyGuard.damage);
        //}
        if (attackHitbox)
        {
            TakeDamage(attackHitbox.Damage);//        change later***********************
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthUI.UpdateHearts(currentHealth);
    }

    void ResetHealth()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
    }

    private void TakeDamage(int damage)
    {
        if(isImmune || isPlayerDie) return;

        currentHealth -= damage;
        audioManager.PlayerSFX(audioManager.PlayerHit);
        healthUI.UpdateHearts(currentHealth);

        StartCoroutine(ImmuneCoroutine());
        //StartCoroutine(FlashRed());

        if(currentHealth <= 0)
        {
            //player dead
            isPlayerDie = true;
            StartCoroutine(AnimationDie());
        }
    }

    IEnumerator AnimationDie()
    {
        audioManager.PlayerSFX(audioManager.PlayerDead);
        movement2.animator.SetTrigger("die");
        movement2.rb.linearVelocity = new Vector2(0f, 0f);

        yield return null;

        yield return new WaitUntil(() =>
            movement2.animator.GetCurrentAnimatorStateInfo(0).IsName("Die") &&
            movement2.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f
        );

        isPlayerDie = false;
        OnPlayerDied.Invoke();
    }

    private IEnumerator ImmuneCoroutine()
    {
        isImmune = true;

        float timer = 0;

        while (timer < immuneTime)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);

            timer += 0.2f;
        }

        spriteRenderer.color = Color.white;
        isImmune = false;
    }

    //private IEnumerator FlashRed()
    //{
    //    spriteRenderer.color = Color.red;
    //    yield return new WaitForSeconds(0.5f);
    //    spriteRenderer.color = Color.white;
    //}
}
