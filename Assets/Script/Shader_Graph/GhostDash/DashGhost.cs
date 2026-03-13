using System.Collections;
using UnityEngine;

public class DashGhost : MonoBehaviour
{
    [Header("Ghost Settings")]
    public Material ghostMaterial;

    [Header("Color & Glow")]
    public Color ghostColor = Color.white;
    [Range(0f, 3f)]
    public float glowIntensity = 1.5f;
    [Range(0f, 1f)]
    public float startAlpha = 0.8f;

    [Header("Trail Settings")]
    public int ghostCount = 5;            // จำนวนร่างที่เห็นพร้อมกัน
    public float ghostInterval = 0.06f;        // spawn ทุกกี่วินาที
    public float ghostLifetime = 0.3f;         // นานแค่ไหนก่อน fade หมด

    private SpriteRenderer playerSprite;
    private Coroutine spawnRoutine;

    private static readonly int AlphaProp = Shader.PropertyToID("_Alpha");
    private static readonly int ColorProp = Shader.PropertyToID("_GhostColor");
    private static readonly int GlowProp = Shader.PropertyToID("_GlowIntensity");

    void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void OnDestroy()
    {
        StopGhost();
    }

    // ── เรียกตอนเริ่ม Dash ──
    public void StartGhost()
    {
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    // ── เรียกตอน Dash จบ ──
    public void StopGhost()
    {
        if (spawnRoutine == null) return;
        StopCoroutine(spawnRoutine);
        spawnRoutine = null;
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnGhost();
            yield return new WaitForSeconds(ghostInterval);
        }
    }

    private void SpawnGhost()
    {
        GameObject ghost = new GameObject("DashGhost");
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;
        ghost.transform.localScale = transform.localScale;

        SpriteRenderer sr = ghost.AddComponent<SpriteRenderer>();
        sr.sprite = playerSprite.sprite;
        sr.flipX = playerSprite.flipX;
        sr.sortingLayerID = playerSprite.sortingLayerID;
        sr.sortingOrder = playerSprite.sortingOrder - 1;

        Material mat = new Material(ghostMaterial);
        mat.SetColor(ColorProp, ghostColor);
        mat.SetFloat(AlphaProp, startAlpha);
        mat.SetFloat(GlowProp, glowIntensity);
        sr.material = mat;

        StartCoroutine(FadeAndDestroy(sr, mat, ghost));
    }

    private IEnumerator FadeAndDestroy(SpriteRenderer sr, Material mat, GameObject ghost)
    {
        float elapsed = 0f;

        while (elapsed < ghostLifetime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / ghostLifetime;

            // Ease out — ร่างแรกหายช้า ร่างท้ายหายเร็ว
            float alpha = Mathf.Lerp(startAlpha, 0f, t * t);
            mat.SetFloat(AlphaProp, alpha);

            // glow ลดตาม alpha ด้วย
            mat.SetFloat(GlowProp, glowIntensity * (1f - t));

            yield return null;
        }

        Destroy(mat);
        Destroy(ghost);
    }
}