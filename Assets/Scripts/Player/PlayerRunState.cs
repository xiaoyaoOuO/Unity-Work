using System;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerRunState : IState
{
    public PlayerRunState(Player player):base(player) {
        stateName = "Run";
        state = State.Run; 
    }
    public override State OnUpdate()
    {
        if(player.canDash) {
            return State.Dash;
        }
        if(player.canRoll) {
            return State.Roll; 
        }
        if (player.canGrab){
            return State.Wall;
        }
        if (player.canAttack)
        { // Check if the player is attacking{
            return State.RunAttack; // Return to attack state
        }
        if(player.canJump) {
            return State.Jump; 
        }
        if(player.IsGrounded() == false) { // Check if the player is grounded{
            return State.Air; // Return to air state
        }
        float xInput = Input.GetAxisRaw("Horizontal");
        if(xInput == 0) {
            return State.Idle;
        }else{
            player.SetVelocity(new Vector2(xInput * player.speed, player.getVelocity().y));
        }
        return State.Run;
    }

    public override void OnEnter() {
        base.OnEnter();
    }
    public override void OnExit() {
       base.OnExit();     
    }
}