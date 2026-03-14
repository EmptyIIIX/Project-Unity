using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth2 : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public HealthUI healthUI;
    private SpriteRenderer spriteRenderer;
    public static event Action OnPlayerDied;
    private DamgeFlash _damgeFlash;

    bool isImmune;
    public float immuneTime = 1f;
    [Header("Damage VFX")]
    public ParticleSystem damageVFXPrefab;
    public static event Action OnPlayerDied;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _damgeFlash = GetComponent<DamgeFlash>();
    }
    void Start()
    {
        ResetHealth();

        spriteRenderer = GetComponent<SpriteRenderer>();
        GameController.OnReset += ResetHealth;
        HoldToLoadLevel.OnHoldComplete += Heal;
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
        if(isImmune) return;

        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);
        _damgeFlash.CallDamgeFlash();

        StartCoroutine(ImmuneCoroutine());
        //StartCoroutine(FlashRed());

        if(currentHealth <= 0)
        {
            //player dead
            OnPlayerDied.Invoke();
        }
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
