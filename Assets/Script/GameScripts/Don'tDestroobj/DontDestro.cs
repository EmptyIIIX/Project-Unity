using UnityEngine;

public class DontDestro : MonoBehaviour
{
    [SerializeField] static DontDestro Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (Instance != null)
                DontDestroyOnLoad(Instance);
        }
        else Destroy(gameObject);
    }
}
