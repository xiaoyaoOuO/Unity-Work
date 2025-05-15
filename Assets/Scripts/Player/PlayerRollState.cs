using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : IState
{
    public PlayerRollState(Player player) : base(player)
    {
        state = State.Roll;
        stateName = "Roll";
    }

    public override void OnEnter()
    {
        base.OnEnter();
        int direction = player.facing == Facing.Right ? 1 : -1;
        Vector2 newSpeed = new Vector2(Math.Max(player.rb.velocity.x,player.rollSpeed), 0);
        player.rb.velocity = newSpeed * direction;
        triggerCalled = false; // Reset the trigger called flag
    }

    public override State OnUpdate()
    {
        if(triggerCalled)
            return State.Idle; 
        return State.Roll;
    }

    public override void AnimationEndTrigger()
    {
        triggerCalled = true;
    }
}
