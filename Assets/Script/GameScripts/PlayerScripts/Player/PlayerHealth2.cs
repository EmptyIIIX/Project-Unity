using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth2 : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public HealthUI healthUI;

    private SpriteRenderer spriteRenderer;
    private DamgeFlash _damgeFlash;

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
        EnemyGuardMovement enemyGuard = collision.GetComponent<EnemyGuardMovement>();
        if (enemyGuard)
        {
            TakeDamage(enemyGuard.damage);
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
        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);
        _damgeFlash.CallDamgeFlash();

        if (currentHealth <= 0)
        {
            //player dead
            OnPlayerDied.Invoke();
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            _damgeFlash.CallDamgeFlash();
            Debug.Log("Osu");
        }
    }
}
