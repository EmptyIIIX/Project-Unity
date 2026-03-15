using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (CheckpointManager.Instance == null)
        {
            Debug.LogError("ไม่มี CheckpointManager ใน Scene!");
            return;
        }

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        StartCoroutine(RespawnWithFade(other));
    }

    IEnumerator RespawnWithFade(Collider2D other)
    {
        Transform playerTransform = other.transform;
        Vector3 respawn = CheckpointManager.Instance.GetRespawnPoint(playerTransform.position);

        // ✅ Debug ตรงนี้ก่อน
        Debug.Log($"TransitionManager: {TransitionManager.Instance}");
        Debug.Log("Starting FadeIn...");

        if (TransitionManager.Instance != null)
            yield return StartCoroutine(TransitionManager.Instance.FadeIn());
        else
            Debug.LogError("TransitionManager เป็น NULL!");
        // ✅ เช็คว่า Player ยังอยู่ก่อน Respawn
        if (playerTransform != null)
        {
            playerTransform.position = respawn;
            Debug.Log($"Respawned at: {respawn}");
        }

        // Fade สว่าง
        if (TransitionManager.Instance != null)
            yield return StartCoroutine(TransitionManager.Instance.FadeOut());
    }
}