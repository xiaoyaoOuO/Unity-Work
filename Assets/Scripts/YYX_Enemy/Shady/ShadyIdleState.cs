using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyIdleState : IEnemyState
{
    public EnemyState state => EnemyState.Idle;

    public string stateName => "Idle";
    private Shady shady;

    public ShadyIdleState(Shady shady)
    {
        this.shady = shady;
    }

    public void OnEnter()
    {
        shady.rb.velocity = Vector2.zero; // 停止移动
        shady.Ani.SetTrigger("Idle");
    }

    public void OnExit()
    {
        shady.Ani.SetBool("Idle", false);
    }

    public EnemyState OnUpdate()
    {
        if(shady.canAttack)
        {
            return EnemyState.Attack; // 如果可以攻击，切换到攻击状态
        }
        if (shady.FindPlayerDistance() <= shady.chaseDistance)
        {
            return EnemyState.Move; // 如果玩家在追击距离内，切换到移动状态
        }
        return state;
    }
}
