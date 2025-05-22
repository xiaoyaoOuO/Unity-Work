using UnityEngine;

public class PlayerStandingAttackState : IState {
    private bool attackEnd;
    public PlayerStandingAttackState(Player player) : base(player) {
        stateName = "StandingAttack";
        state = State.StandingAttack;
    }

    public override State OnUpdate() {
        if(attackEnd) { 
            attackEnd = false;
            return State.Idle;
        }
        return state;
    }

    public override void OnEnter() {
        base.OnEnter();
        player.attackTimer = player.attackCooldown; // Reset the attack timer
        attackEnd = false; // Reset the attack end flag
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