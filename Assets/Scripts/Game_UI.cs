
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Game_UI : MonoBehaviour
{
    public Player player;
    public int currentHealth;
    public List<Image> healthBars;
    public Slider BulletTimeSlider;

    public void UpdateHealthBars()
    {
        currentHealth = player.playerState.currentHealth;

        for (int i = 0; i < healthBars.Count; i++)
        {
            if (i < currentHealth)
            {
                healthBars[i].enabled = true;
            }
            else
            {
                healthBars[i].enabled = false;
            }
        }
    }

    public void ConsumeBulletTime()
    {
        BulletTimeSlider.value = player.BulletTimer / player.BulletTimeDuration;
    }

    public void ResetBulletTime()
    {
        BulletTimeSlider.value = player.BulletTimeCooldownTimer / player.BulletTimeDuration;
    }
}
