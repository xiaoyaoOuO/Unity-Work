using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck
{
    public float timer;
    public Player player;
    public JumpCheck(Player player){
        this.player = player;
        timer = 0;
    }

    public void Update(){
       if(player.IsGrounded()){
            timer = player.jumpGraceTime;
        }else if(timer > 0){
            timer -= Time.deltaTime;
        }   
    }

    public bool AllowJump(){
        return timer > 0;
    }


}
