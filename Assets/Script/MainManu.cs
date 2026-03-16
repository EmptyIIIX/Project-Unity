using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // โหลด Scene ด้วย Index
    public void LoadScenePlay()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadSceneByPlay()
    {
        SceneManager.LoadScene("Main_Scene");
    }
}