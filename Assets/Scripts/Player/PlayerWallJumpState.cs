using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : IState
{
    private float forceMoveTimer;
    private float wallBoostSpeed;
    private float forceMoveDuration;
    public PlayerWallJumpState(Player player) : base(player)
    {
        this.stateName = "WallJump";
        this.state = State.WallJump;
        forceMoveDuration = player.wallJumpDuration;
        wallBoostSpeed = player.wallJumpBoostSpeed;
    }

    public override State OnUpdate()
    {
        forceMoveTimer -= Time.deltaTime;
        if(player.canGrab){ return State.Wall;}
        if (forceMoveTimer <= 0) { return State.Air; }
        return state;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        forceMoveTimer = forceMoveDuration;
        int direction = player.facing == Facing.Right ? 1 : -1;
        player.rb.velocity = new Vector2(wallBoostSpeed * -direction, player.wallJumpForce);
        player.OnFlip();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
