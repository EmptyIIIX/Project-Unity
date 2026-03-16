
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int progressSoulAmount;
    public Slider progressSoulSlider;
    public GameObject player;
    public GameObject LoadCanvas;
    public List<GameObject> levels;
    private int currentLevelIndex = 0;
    public GameObject gameOverScreen;
    public static event Action OnReset;

    void Start()
    {
        progressSoulAmount = 0;
        SetSliderValue(0);
        Soul.OnSoulCollect += IncreaseProgressSoulAmount;
        HoldToLoadLevel.OnHoldComplete += LoadHealPlayer;
        PlayerAttack.HitEnemy += absorbSoul;
        PlayerHealth2.OnPlayerDied += GameOverScreen;
        GateToNextLevel.IntoGate += LoadNextLevel;

        if (LoadCanvas != null) LoadCanvas.SetActive(false);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);

    }

    // Unsubscribe ทุก Event ตอน Destroy
    void OnDestroy()
    {
        Soul.OnSoulCollect -= IncreaseProgressSoulAmount;
        HoldToLoadLevel.OnHoldComplete -= LoadHealPlayer;
        PlayerAttack.HitEnemy -= absorbSoul;
        PlayerHealth2.OnPlayerDied -= GameOverScreen;
        GateToNextLevel.IntoGate -= LoadNextLevel;
    }

    // Helper — เช็ค Null ก่อน Set ทุกครั้ง
    void SetSliderValue(float value)
    {
        if (progressSoulSlider != null)
            progressSoulSlider.value = value;
    }

    void GameOverScreen()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResetGame()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
        LoadLevel(0);
        OnReset?.Invoke();
        if (CheckpointManager.Instance != null)
            CheckpointManager.Instance.ResetCheckpoint();
        Time.timeScale = 1;
    }

    void IncreaseProgressSoulAmount(int amount)
    {
        progressSoulAmount += amount;
        SetSliderValue(progressSoulAmount);

        if (progressSoulAmount >= 100)
        {
            Debug.Log("Level Complete");
            if (LoadCanvas != null)
                LoadCanvas.SetActive(true);
        }
    }

    void LoadLevel(int level)
    {
        if (LoadCanvas != null) LoadCanvas.SetActive(false);
        if (levels[currentLevelIndex] != null)
            levels[currentLevelIndex].gameObject.SetActive(false);
        if (levels[level] != null)
            levels[level].gameObject.SetActive(true);

        currentLevelIndex = level;
        //if (player != null)
        //    player.transform.position = SpawnPoint.transform.position;
    }

    void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1)
            ? 0 : currentLevelIndex + 1;
        LoadLevel(nextLevelIndex);
    }

    void LoadHealPlayer(int amount)
    {
        if (LoadCanvas != null) LoadCanvas.SetActive(false);
        progressSoulAmount = 0;
        SetSliderValue(0);
    }

    void absorbSoul()
    {
        progressSoulAmount += 1;
        SetSliderValue(progressSoulAmount);
    }
}
