using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        progressSoulAmount = 0;
        progressSoulSlider.value = 0;
        Soul.OnSoulCollect += IncreaseProgressSoulAmount;
        //HoldToLoadLevel.OnHoldComplete += LoadNextLevel;          have to go into the gate
        HoldToLoadLevel.OnHoldComplete += LoadHealPlayer;
        PlayerAttack.HitEnemy += absorbSoul;
        LoadCanvas.SetActive(false);
        PlayerHealth2.OnPlayerDied += GameOverScreen;
        gameOverScreen.SetActive(false);

        GateToNextLevel.IntoGate += LoadNextLevel;
    }

    void GameOverScreen()
    {
        gameOverScreen.SetActive(true);

        Time.timeScale = 0;     //Time stop###
    }

    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        LoadLevel(0);       //this can use with teleport pole*****************************
        OnReset.Invoke();

        if (CheckpointManager.Instance != null)
            CheckpointManager.Instance.ResetCheckpoint();

        Time.timeScale = 1;     //Time run###
    }
    void IncreaseProgressSoulAmount(int amount)
    {
        progressSoulAmount += amount;
        progressSoulSlider.value = progressSoulAmount;
        if (progressSoulAmount >= 100)
        {
            //Collect Complete
            Debug.Log("Level Complete");

            //this can change to be onGate to boss room and not this method
            LoadCanvas.SetActive(true);
        }
    }

    void LoadLevel(int level)
    {
        LoadCanvas.SetActive(false);

        levels[currentLevelIndex].gameObject.SetActive(false);
        levels[level].gameObject.SetActive(true);
        currentLevelIndex = level;
        
        if (player != null && CheckpointManager.Instance != null)
        player.transform.position = CheckpointManager.Instance.GetRespawnPoint(levels[level].transform.position);
    }

    void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
        LoadLevel(nextLevelIndex);
    }

    void LoadHealPlayer(int amount)
    {
        LoadCanvas.SetActive(false);
        progressSoulAmount = 0;
        progressSoulSlider.value = 0;
    }

    void absorbSoul()
    {
        progressSoulAmount += 1;
        progressSoulSlider.value += 1;
    }
}
