using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [SerializeField] Material transitionMaterial;
    [SerializeField] float duration = 1f;
    [SerializeField] GameObject transitionCanvas; // ลาก TransitionShader GameObject มาใส่

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ทำให้ Canvas ไม่ถูก Destroy ด้วย
            if (transitionCanvas != null)
                DontDestroyOnLoad(transitionCanvas);

            transitionMaterial.SetFloat("_Progress", 0f);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        transitionMaterial.SetFloat("_Progress", 0f);
    }

    public void NextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(DoTransition(nextIndex));
    }

    public void TeleportToBossRoom(string sceneName)
    {
        StartCoroutine(DoTransitionByName(sceneName));
    }

    IEnumerator DoTransition(int sceneIndex)
    {
        // ดำก่อน
        yield return StartCoroutine(Fade(0f, 255f));

        // Load Scene
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f) yield return null;

        op.allowSceneActivation = true;
        yield return null;

        // สว่าง
        yield return StartCoroutine(Fade(255f, 0f));
    }

    IEnumerator DoTransitionByName(string sceneName)
    {
        yield return StartCoroutine(Fade(0f, 255f));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f) yield return null;

        op.allowSceneActivation = true;
        yield return null;

        yield return StartCoroutine(Fade(255f, 0f));
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float val = Mathf.Lerp(from, to, t / duration);
            transitionMaterial.SetFloat("_Progress", val);
            yield return null;
        }
        transitionMaterial.SetFloat("_Progress", to);
    }
}