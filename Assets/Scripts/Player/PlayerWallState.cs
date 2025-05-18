using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallState : IState
{
    private FiniteStateMachine<IState> fsm;
    private WallBoost wallBoost;
    PlayerWallIdleState playerWallIdleState;
    PlayerWallClimbState playerWallClimbState;
    PlayerCrossWallState playerCrossWallState;
    public PlayerWallState(Player player) : base(player)
    {
        this.stateName = "Wall";
        this.state = State.Wall;
        fsm = new FiniteStateMachine<IState>();
        wallBoost = new WallBoost();
        playerWallIdleState = new PlayerWallIdleState(player);
        playerWallClimbState = new PlayerWallClimbState(player);
        playerCrossWallState = new PlayerCrossWallState(player);
        fsm.AddState(playerWallClimbState);
        fsm.AddState(playerWallIdleState);
        fsm.AddState(playerCrossWallState);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        int dir = player.facing == Facing.Right ? 1 : -1;
        RaycastHit2D hit = player.RightWallCheck();
        if (hit)
        {
            player.transform.position += hit.distance * Vector3.right * dir;
        }
        fsm.Initialize(playerWallIdleState);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override State OnUpdate()
    {
        if (GameInput.IsJumpPressed())
        {
            fsm.CurrentState.OnExit();
            return State.WallJump;
        }
        fsm.Update();
        if (fsm.CurrentState == playerCrossWallState)
        {
            // Debug.Log("CrossWall");
            if (playerCrossWallState.CrossWallEnd)
            {
                fsm.CurrentState.OnExit();
                player.transform.position = player.CorrectCrossWall.transform.position;
                playerCrossWallState.CrossWallEnd = false;
                return State.Idle;
            }
            return state;
        }
        if (!player.canGrab)
        {
            fsm.CurrentState.OnExit();
            return State.Idle;
        }
        return state;
    }

    public override void AnimationEndTrigger()
    {
        fsm.CurrentState.AnimationEndTrigger();
    }
}
