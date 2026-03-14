using System.Collections;
using UnityEngine;

public class DashGhost : MonoBehaviour
{
    [Header("Ghost Settings")]
    public Material ghostMaterial;
    public float ghostLifetime = 0.35f;
    public float spawnInterval = 0.03f;
    [Range(0f, 1f)]
    public float startAlpha = 1f;

    private bool _isSpawning;

    public void StartAfterimage()
    {
        if (!_isSpawning)
            StartCoroutine(SpawnLoop());
    }

    public void StopAfterimage() => _isSpawning = false;

    IEnumerator SpawnLoop()
    {
        _isSpawning = true;
        while (_isSpawning)
        {
            SpawnGhost();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnGhost()
    {
        SpriteRenderer[] allRenderers = GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer original in allRenderers)
        {
            if (original.sprite == null) continue;

            var ghost = new GameObject("Ghost");
            ghost.transform.SetPositionAndRotation(
                original.transform.position,
                original.transform.rotation);
            ghost.transform.localScale = original.transform.lossyScale;

            var sr = ghost.AddComponent<SpriteRenderer>();
            sr.sprite = original.sprite;
            sr.flipX = original.flipX;
            sr.flipY = original.flipY;
            sr.sortingLayerName = original.sortingLayerName;
            sr.sortingOrder = original.sortingOrder - 1;

            // ตั้ง alpha เริ่มต้นผ่าน color โดยตรง
            Color c = original.color;
            c.a = startAlpha;
            sr.color = c;

            ghost.AddComponent<GhostFader>().Init(sr, ghostLifetime, startAlpha);
        }
    }
}