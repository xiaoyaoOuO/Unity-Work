
using UnityEngine;
using System.Collections;

public class PlayerRollAttackState : IState
{
    private bool rollAttackEnd;
    private float acceleration;
    public PlayerRollAttackState(Player player) : base(player)
    {
        state = State.RollAttack;
        stateName = "RollAttack";
        acceleration = 20f;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        rollAttackEnd = false;
    }

    public override State OnUpdate()
    {
        if (rollAttackEnd)
        {
            return State.Idle;
        }
        Vector2 velocity = player.rb.velocity;
        velocity.x = Mathf.MoveTowards(velocity.x, 0, acceleration * Time.deltaTime);
        player.rb.velocity = velocity;
        Debug.Log(player.rb.velocity);
        return state;
    }

    public override void AnimationEndTrigger()
    {
        rollAttackEnd = true;
    }
}
