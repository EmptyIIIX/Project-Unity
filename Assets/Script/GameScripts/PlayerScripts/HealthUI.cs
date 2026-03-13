using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image HealthPrefab;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    private List<Image> hearts = new List<Image>();

    public void SetMaxHearts(int maxHearts)
    {
        foreach(Image heaert in hearts)
        {
            Destroy(heaert.gameObject);
        }

        hearts.Clear();

        for (int i = 0; i < maxHearts; i++)
        {
            Image newHeart = Instantiate(HealthPrefab, transform);
            newHeart.sprite = fullHeartSprite;
            hearts.Add(newHeart);
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if(i < currentHealth)
            {
                hearts[i].sprite = fullHeartSprite;
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite;
            }
        }
    }
}
