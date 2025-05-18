using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Game_UI : MonoBehaviour
{
    public Player player;
    public int currentHealth;
    public List<SpriteRenderer> healthBars;

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
}
