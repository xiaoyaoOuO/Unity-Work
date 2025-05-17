using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallState : IState
{
    private FiniteStateMachine<IState> fsm;
    private WallBoost wallBoost;
    public PlayerWallState(Player player) : base(player)
    {
        this.stateName = "Wall";
        this.state = State.Wall;
        fsm = new FiniteStateMachine<IState>();
        wallBoost = new WallBoost();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        fsm.AddState(new PlayerWallClimbState(player));
        PlayerWallIdleState playerWallIdleState = new PlayerWallIdleState(player);
        fsm.AddState(playerWallIdleState);
        fsm.Initialize(playerWallIdleState);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override State OnUpdate()
    {
        wallBoost.Update();
        fsm.Update();
        // if (GameInput.IsJumpPressed() && wallBoost.timer > 0)
        // {
        //     return State.WallJump;
        // }
        if (!GameInput.IsGrabPressed())
        {
            fsm.CurrentState.OnExit();
            return State.Idle;
        }
        return state;
    }
}
