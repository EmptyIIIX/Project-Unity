using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;
    [SerializeField] Material transitionMaterial;
    [SerializeField] float duration = 1f;
    [SerializeField] GameObject transitionCanvas;

    PlayerMovement2 player;
    PlayerHealth2 playerHp;
    [SerializeField]private int Hp;//                      have fixed
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (transitionCanvas != null)
                DontDestroyOnLoad(transitionCanvas);
            transitionMaterial.SetFloat("_Progress", 0f);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        transitionMaterial.SetFloat("_Progress", 0f);
        FindPlayer();
        Hp = playerHp.currentHealth;//                      have fixed
    }

    // หา Player ทุกครั้งที่ Load Scene ใหม่
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer();
        transitionMaterial.SetFloat("_Progress", 0f);
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.GetComponent<PlayerMovement2>();
            playerHp = playerObj.GetComponent<PlayerHealth2>();

        if (Hp > 0 && playerHp != null)
        {
            playerHp.currentHealth = Hp;
            playerHp.healthUI.UpdateHearts(Hp); // อัพเดท UI ด้วย
        }
    }

    void SetPlayerMove(bool canMove)
    {
        if (player != null)
            player.iscanMove = canMove;
    }

    public void NextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(DoTransition(nextIndex));
    }

    public void CurrentScene(string sceneName)
    {
        StartCoroutine(DoTransitionByName(sceneName));
    }

    IEnumerator DoTransition(int sceneIndex)
    {
        SetPlayerMove(false);
        Hp = playerHp.currentHealth;
        yield return StartCoroutine(Fade(0f, 255f));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f) yield return null;
        op.allowSceneActivation = true;
        yield return null;
        FindPlayer();
        yield return StartCoroutine(Fade(255f, 0f));
        SetPlayerMove(true);
    }

    IEnumerator DoTransitionByName(string sceneName)
    {
        SetPlayerMove(false);
        Hp = playerHp.currentHealth;
        yield return StartCoroutine(Fade(0f, 255f));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f) yield return null;
        op.allowSceneActivation = true;
        FindPlayer();
        yield return null;

        yield return StartCoroutine(Fade(255f, 0f));
        SetPlayerMove(true);
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

    public IEnumerator FadeIn()
    {
        SetPlayerMove(false);
        yield return StartCoroutine(Fade(0f, 255f));
    }

    public IEnumerator FadeOut()
    {
        yield return StartCoroutine(Fade(255f, 0f));
        SetPlayerMove(true);
    }
}