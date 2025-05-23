using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrossWallState : IState
{
    public bool CrossWallEnd;
    public PlayerCrossWallState(Player player) : base(player)
    {
        this.stateName = "CrossWall";
        this.state = State.CrossWall;
        CrossWallEnd = false;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        CrossWallEnd = false;
    }

    public override State OnUpdate()
    {
        player.ZeroVelocity();
        return state;
    }

    public override void AnimationEndTrigger()
    {
        CrossWallEnd = true;
    }
}
