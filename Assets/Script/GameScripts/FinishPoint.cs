using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [Header("Press E UI")]
    [SerializeField] GameObject pressEUI;

    bool playerNearby = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            pressEUI.SetActive(false);
            SceneController.instance.NextLevel();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerNearby = true;
        pressEUI.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerNearby = false;
        pressEUI.SetActive(false);
    }
}