using System;
using UnityEngine;

public enum AttackDirection : int
{
    None = 0,
    Right,
    Up,
    Down, 
};
public class PlayerAttackState : IState {
    public bool attackEnd;
    public PlayerAttackState(Player player) : base(player) {
        stateName = "Attack";
        state = State.Attack;
    }

    public override State OnUpdate() {
        if(attackEnd) { 
            attackEnd = false;
            return State.Idle; // Return to idle state
        }
        return state;
    }

    public override void OnEnter() {
        // player.dashCount--;   TODO : Dash count
        base.OnEnter();
        attackEnd = false;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world space
        Vector2 attackDirection = (mousePosition - (Vector2)player.transform.position).normalized; // Calculate the direction from the player to the mouse position

        int Dir;    //获取人物朝向
        if(player.facing == Facing.Right) { // Right direction
            Dir = 1;
        } else { // Left direction
            Dir = -1;   
        }

        float angle = Vector2.SignedAngle(Vector2.right * Dir, attackDirection); // Calculate the angle between the player and the mouse position
        
        Dir = (int)AttackDirection.None; //Dir转变为攻击方向 

        if(angle > -90 && angle < 90) { // Right direction
            if(angle > 45) { // Up direction
                Dir = (int)AttackDirection.Up;
            }else if(angle < -45) { // Down direction
                Dir = (int)AttackDirection.Down;
            }else { // Right direction
                Dir = (int)AttackDirection.Right;
            }  
        }else{
            player.OnFlip(); // Flip the player if the mouse is on the left side of the player
            if(angle < 135 && angle >0){
                Dir = (int)AttackDirection.Up; // Up direction
            }else if(angle > -135 && angle < 0){ // Down direction
                Dir = (int)AttackDirection.Down; // Down direction
            }else { // Right direction
                Dir = (int)AttackDirection.Right; // Right direction
            }
        }
        player.SetAnimation("AttackDir",Dir);
    }

    public override void OnExit() {
        base.OnExit(); 
        player.attackTimer = player.attackCooldown; // Reset the attack timer
    }

    public override void AnimationEndTrigger() { // 攻击结束时调用的函数
        attackEnd = true; // 攻击结束
    }

}