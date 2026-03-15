using System.Collections;
using UnityEngine;

public class GhostFader : MonoBehaviour
{
    public void Init(SpriteRenderer sr, float lifetime, float startAlpha)
    {
        StartCoroutine(Fade(sr, lifetime, startAlpha));
    }

    IEnumerator Fade(SpriteRenderer sr, float lifetime, float startAlpha)
    {
        float elapsed = 0f;
        Color c = sr.color;

        while (elapsed < lifetime)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, 0f, elapsed / lifetime);
            sr.color = c;
            yield return null;
        }
        Destroy(gameObject);
    }

}