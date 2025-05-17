using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : IState
{
    public PlayerWallJumpState(Player player) : base(player)
    {
        this.stateName = "WallJump";
        this.state = State.WallJump;
    }

    public override State OnUpdate()
    {
        return state;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
