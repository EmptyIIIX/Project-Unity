using System.Transactions;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        Vector3 respawn = CheckpointManager.Instance.GetRespawnPoint(other.transform.position);
        other.transform.position = respawn;
    }
}

//public class Spike : MonoBehaviour
//{
//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (!other.CompareTag("Player")) return;

//        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
//        if (rb != null) rb.linearVelocity = Vector2.zero;

//        StartCoroutine(RespawnWithFade(other));
//    }

//    IEnumerator RespawnWithFade(Collider2D other)
//    {
//        // Fade ดำ
//        yield return StartCoroutine(TransitionManager.Instance.FadeIn());

//        // Respawn
//        Vector3 respawn = CheckpointManager.Instance.GetRespawnPoint(other.transform.position);
//        other.transform.position = respawn;

//        // Fade สว่าง
//        yield return StartCoroutine(TransitionManager.Instance.FadeOut());
//    }
//}