using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallIdleState : IState
{
    public PlayerWallIdleState(Player player) : base(player)
    {
        this.stateName = "WallIdle";
        this.state = State.WallIdle;
    }

    public override State OnUpdate()
    {
        if (!player.HeadWallCheck())
        {
            return State.CrossWall;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            return State.WallClimb;
        }
        player.ZeroVelocity();
        return state;
    }
}
