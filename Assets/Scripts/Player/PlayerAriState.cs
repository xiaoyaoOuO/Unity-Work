using UnityEngine;

public class PlayerAirState : IState {
    public PlayerAirState(Player player) : base(player) {
        stateName = "Jump";
        state = State.Air;
    }
    public override State OnUpdate() {
        float xInput = Input.GetAxisRaw("Horizontal");
        player.SetVelocity(new Vector2(xInput * player.speed * 0.8f, player.getVelocity().y));
        if(player.canDash){
            return State.Dash;
        }
        if (player.canGrab)
        {
            return State.Wall;
        }
        if (player.canJump)
        {
            return State.Jump;
        }
        if(player.canAttack) {
            return State.Attack; 
        }
        if(player.IsGrounded()) {
            player.effectController.PlayerLandFX(player.animator.transform.position);
            return State.Idle;
        }
        player.SetAnimation("yVelocity", player.getVelocity().y);
        return state;
    }

    public override void OnEnter() {
        base.OnEnter();
    }

    public override void OnExit() {
        base.OnExit();
    }
}