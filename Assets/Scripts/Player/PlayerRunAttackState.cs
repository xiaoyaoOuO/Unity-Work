using UnityEngine;

public class PlayerRunAttackState : IState {
    private bool attackEnd;
    private int slashAttackCount;
    public PlayerRunAttackState(Player player) : base(player)
    {
        stateName = "RunAttack";
        state = State.RunAttack;
        slashAttackCount = 0;
    }

    public override State OnUpdate() {
        if (player.canDash)
        {
            return State.DashAttack;
        }
        if (player.canRoll)
        {
            return State.Roll;
        }
        if (attackEnd)
            {
                attackEnd = false;
                return State.Run; // Return to idle state
            }
        float dir = Input.GetAxisRaw("Horizontal"); // Get the horizontal input direction
        player.SetVelocity(new Vector2(dir * player.speed, player.rb.velocity.y)); 
        return state;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.attackTimer = player.attackCooldown; // Reset the attack timer
        attackEnd = false; // Reset the attack end flag
        slashAttackCount = (slashAttackCount + 1) & 1;
        player.SetAnimation("RunSlash", slashAttackCount);
    }

    public override void OnExit() {
        base.OnExit();
    }

    public override void AnimationEndTrigger() {
       attackEnd = true; // Set the attackEnd flag to true when the animation ends
    }

    public override void AnimationAttackTrigger(Collider2D collider = null) {
        base.AnimationAttackTrigger(player.RightAttackCollider);
    }
}