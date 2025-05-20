using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public int maxHealth;
    public int currentHealth;
    Player player;

    public PlayerState(Player player, int maxHealth)
    {
        this.player = player;
        this.maxHealth = maxHealth;
    }
    public void OnHit()
    {
        currentHealth--;
        player.effectController.CameraShake(player.facing == Facing.Right ? Vector2.right : Vector2.left);
        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead");        //TODO : 死亡界面
        }
        
    }
}
