using UnityEngine;

public class FullScreenDamageEffect : MonoBehaviour
{
    [Header("Material จาก Renderer Feature")]
    [SerializeField] private Material damageMaterial; // ลาก FullScreenDamageMAT เข้ามา

    [Header("Player Reference")]
    [SerializeField] private PlayerHealth2 player;

    [Header("Settings")]
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float maxPower = 5f;

    private void Update()
    {
        if (player == null || damageMaterial == null) return;

        float percent = (float)player.currentHealth / player.maxHealth;

        if (percent <= 0.8f)
        {
            float t = 1f - (percent / 0.8f);
            float pulse = (Mathf.Sin(Time.time * pulseSpeed * Mathf.PI * 2f) + 1f) / 2f;
            float newPower = Mathf.Lerp(t * 1.5f, t * maxPower, pulse);

            damageMaterial.SetFloat("_Power", newPower);
        }
        else
        {
            damageMaterial.SetFloat("_Power", 0f);
        }
    }
}