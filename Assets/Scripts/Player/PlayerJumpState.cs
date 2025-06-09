using UnityEngine;

public class PlayerJumpState : IState
{
    public PlayerJumpState(Player player) : base(player)
    {
        stateName = "Jump";
        state = State.Jump;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce); // Set the player's velocity to the jump force in the y direction
        player.effectController.PlayerJumpFX(player.animator.transform.position);
    }

    public override State OnUpdate()
    {
        if(player.canDash){ // If the player presses the dash button, change to the dash state
            return State.Dash;        
        }
        if (player.canGrab) // If the player presses the grab button, change to the wall slide state
        {
            return State.Wall;
        }
        if (player.canAttack)
        { // If the player presses the attack button, change to the attack state
            return State.Attack;
        }
        if (player.IsGrounded())
        {
            player.effectController.PlayerLandFX(player.animator.transform.position);
            return State.Idle;
        }
        if (player.rb.velocity.y < 0)
                return State.Air;
        player.SetAnimation("yVelocity", player.rb.velocity.y);
        return state;
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}