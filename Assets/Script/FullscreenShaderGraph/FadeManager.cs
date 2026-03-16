using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [Header("Fade Settings")]
    public Image fadeImage;

    [Range(0.1f, 3f)]
    public float fadeInDuration = 1f;   // เวลา Fade เข้า (โปร่งใส → ขาว)

    [Range(0.1f, 3f)]
    public float fadeOutDuration = 1f;  // เวลา Fade ออก (ขาว → โปร่งใส)

    [Range(0f, 5f)]
    public float delayAfterFade = 0.5f; // รอหลัง Fade เสร็จก่อน LoadScene

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetAlpha(0f);
    }

    // โหลดด้วย Index
    public void LoadSceneByIndex(int index)
    {
        StartCoroutine(FadeAndLoad(() =>
            UnityEngine.SceneManagement.SceneManager.LoadScene(index)));
    }

    // โหลดด้วย Name
    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(FadeAndLoad(() =>
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName)));
    }

    // Fade ออก (ใช้ตอนเข้า Scene ใหม่)
    public void FadeOut(Action onComplete = null)
    {
        StartCoroutine(DoFade(1f, 0f, fadeOutDuration, onComplete));
    }

    IEnumerator FadeAndLoad(Action loadAction)
    {
        // Step 1 : Fade เข้า (โปร่งใส → ขาว)
        yield return StartCoroutine(DoFade(0f, 1f, fadeInDuration, null));

        // Step 2 : รอหลัง Fade เสร็จ
        if (delayAfterFade > 0f)
            yield return new WaitForSeconds(delayAfterFade);

        // Step 3 : โหลด Scene
        loadAction.Invoke();
    }

    IEnumerator DoFade(float from, float to, float duration, Action onComplete)
    {
        float timer = 0f;
        SetAlpha(from);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            SetAlpha(Mathf.Lerp(from, to, timer / duration));
            yield return null;
        }

        SetAlpha(to);
        onComplete?.Invoke();
    }

    void SetAlpha(float alpha)
    {
        if (fadeImage == null) return;
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}