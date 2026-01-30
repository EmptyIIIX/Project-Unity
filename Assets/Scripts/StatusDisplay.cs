using UnityEngine;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    public PlayerHealth playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        health = playerHealth.health;
        maxHealth = playerHealth.maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            // player's HP on the time
            //if(i < health)
            //{
            //    hearts[i].sprite = fullHeart;
            //}
            //else
            //{
            //    hearts[i].sprite = emptyHeart;
            //}

            // max health display
            //if(i < maxHealth)
            //{
            //    hearts[i].enabled = true;
            //}
            //else
            //{
            //    hearts[i].enabled = false;
            //}
        }
    }
}
