using UnityEngine;

public class PlayerIdleState : IState {

    public PlayerIdleState(Player player) : base(player) {
        stateName = "Idle";
        state = State.Idle;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override State OnUpdate()
    {
        if(player.canDash) {
            return State.Dash;
        }
        if(player.canRoll) {
            return State.Roll; 
        }
        if(player.canGrab) {
            return State.Wall;
        }
        if (player.canAttack)
        {
            return State.StandingAttack;
        }
        if(player.IsGrounded() == false) {
           return State.Air; 
        }
        if(player.canJump) {
            return State.Jump;
        }
        if(Input.GetAxisRaw("Horizontal") != 0) {
            return State.Run;
        }
        if(player.rb.velocity != Vector2.zero) {
            player.rb.velocity = new Vector2(Mathf.Lerp(player.rb.velocity.x, 0, player.acceleration * Time.deltaTime), player.rb.velocity.y);
        }
        return state;
    }
}