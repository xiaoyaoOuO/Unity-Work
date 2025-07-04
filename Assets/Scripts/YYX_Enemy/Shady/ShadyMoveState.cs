using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyMoveState : IEnemyState
{
    public EnemyState state => EnemyState.Move;

    public string stateName => "Moving";

    private Shady shady;

    public ShadyMoveState(Shady shady)
    {
        this.shady = shady;
    }

    public void OnEnter()
    {
        shady.Ani.SetTrigger("Move");
        shady.Chase();
    }

    public void OnExit()
    {
        shady.Ani.SetBool("Move", false);
    }

    public EnemyState OnUpdate()
    {
        if(shady.canAttack)
        {
            return EnemyState.Attack;
        }
        if (shady.FindPlayerDistance() > shady.chaseDistance)
        {
            return EnemyState.Idle;
        }
        shady.Chase();
        return state;
    }
}
