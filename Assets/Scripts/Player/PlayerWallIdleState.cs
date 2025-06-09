
using UnityEngine;

public class PlayerWallIdleState : IState
{
    private GameObject dust;
    public PlayerWallIdleState(Player player) : base(player)
    {
        this.stateName = "WallIdle";
        this.state = State.WallIdle;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        dust = player.effectController.PlayerWallSlideFX(player.animator.transform.position);
    }

    public override State OnUpdate()
    {
        if (!player.HeadWallCheck() && player.RightWallCheck())
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

    public override void OnExit()
    {
        base.OnExit();
        GameObject.Destroy(dust); 
    }
}
