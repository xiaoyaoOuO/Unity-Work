using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : IState
{
    private bool RollAttack;
    private bool CanChangeToAttack;
    private Vector2 colliderDefaultScale;
    private Vector2 colliderDefaultOffset;
    private float colliderYOffset = -0.4f;          //写死的数据
    private float colliderYSize = 0.7f;
    public PlayerRollState(Player player) : base(player)
    {
        state = State.Roll;
        stateName = "Roll";
    }

    public override void OnEnter()
    {
        base.OnEnter();
        int direction = player.facing == Facing.Right ? 1 : -1;
        Vector2 newSpeed = new Vector2(Math.Max(player.rb.velocity.x, player.rollSpeed), 0);
        player.rb.velocity = newSpeed * direction;
        triggerCalled = false; // Reset the trigger called flag
        RollAttack = false;
        CanChangeToAttack = false;

        colliderDefaultScale = player.boxCollider.size; 
        colliderDefaultOffset = player.boxCollider.offset;
        player.boxCollider.size = new Vector2(colliderDefaultScale.x, colliderYSize); 
        player.boxCollider.offset = new Vector2(player.boxCollider.offset.x, colliderYOffset); 
    }

    public override State OnUpdate()
    {
        if (GameInput.IsAttackPressed())
        {
            RollAttack = true;
        }
        if (RollAttack)
        {
            return State.RollAttack;
        }
        if (triggerCalled)
        {
            return State.Idle;
        }
        return State.Roll;
    }

    public override void AnimationEndTrigger()
    {
        triggerCalled = true;
    }

    public override void FirstAnimationTrigger()
    {
        CanChangeToAttack = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        player.boxCollider.size = colliderDefaultScale;
        player.boxCollider.offset = colliderDefaultOffset;
    }
}
