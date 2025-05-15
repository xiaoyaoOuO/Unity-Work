using System;
using UnityEngine;

public enum State {
     Idle, 
     Run, 
     Air,
     Attack, 
     Hit, 
     Die,
     Dash,
     Jump,
     RunAttack,
     StandingAttack,
     DashAttack,
     Roll,
}
public abstract class IState
{
    public State state{get; protected set;}
    public Player player;
    public String stateName {get; protected set;}
    public IState(Player player) { this.player = player; }
    public bool triggerCalled{get; protected set;}
    
    public virtual void OnEnter()
    {
        player.SetAnimation(stateName);
    }
    public virtual void OnExit()
    {
        player.SetAnimation(stateName, false);
    }
    public abstract State OnUpdate();

    public virtual void AnimationEndTrigger() { triggerCalled = true; }
    public virtual void AnimationAttackTrigger()
    {
    }

}