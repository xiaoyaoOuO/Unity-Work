using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeadState : IState
{
    public PlayerDeadState(Player player) : base(player)
    {
        state = State.Die;
        stateName = "Die";
    }
    public override void OnEnter()
    {
        base.OnEnter();
        player.ZeroVelocity();
        base.triggerCalled = false;
    }

    public override State OnUpdate()
    {
        player.ZeroVelocity();
        if (base.triggerCalled)
        {
            player.StartCoroutine(player.Die());
        }
        return state;
    }

    public override void AnimationEndTrigger()
    {
        base.AnimationEndTrigger();
    }

}
