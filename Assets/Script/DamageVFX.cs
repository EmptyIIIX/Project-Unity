using UnityEngine;

public class DamageVFX : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] private ParticleSystem enemyDamagePro; // ลาก Prefab ใส่ได้เลย

    public void PlayDamageVFX()
    {
        if (enemyDamagePro == null)
        {
            Debug.LogError("enemyDamagePro is NULL!");
            return;
        }

        // Instantiate ที่ตำแหน่ง Player แล้ว Destroy อัตโนมัติ
        ParticleSystem vfx = Instantiate(
            enemyDamagePro,
            transform.position,   // ตำแหน่ง Player
            Quaternion.identity
        );

        vfx.Play();
        Destroy(vfx.gameObject, vfx.main.duration + 0.1f);
    }
}