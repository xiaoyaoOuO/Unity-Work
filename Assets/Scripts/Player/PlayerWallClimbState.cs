using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : IState
{

    public PlayerWallClimbState(Player player) : base(player)
    {
        this.stateName = "WallClimb";
        this.state = State.WallClimb;
    }

    public override State OnUpdate()
    {
        int climbDirection = Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0);
        if (climbDirection != 0)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, climbDirection * player.wallSlideSpeed);
        }
        else
        {
            player.ZeroVelocity();
            return State.WallIdle;
        }
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
