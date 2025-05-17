using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public int Health;
    Player player;

    public PlayerState(Player player) { this.player = player; }
    public void OnHit()
    {
        Health -= 1;
        player.effectController.CameraShake(player.facing == Facing.Right ? Vector2.right : Vector2.left);
        if (Health <= 0)
        {
            Debug.Log("Player is dead");        //TODO : 死亡界面
        }
    }
}
