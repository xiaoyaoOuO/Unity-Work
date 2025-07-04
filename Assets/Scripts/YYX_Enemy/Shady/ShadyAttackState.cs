using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyAttackState : IEnemyState
{
    public EnemyState state => EnemyState.Attack;

    public string stateName => "Attack";

    private Shady shady;

    public ShadyAttackState(Shady shady)
    {
        this.shady = shady;
    }

    public void OnEnter()
    {
        shady.rb.velocity = Vector2.zero; // 停止移动
        shady.Ani.SetTrigger("Attack");
        shady.isAttack = true; // 设置攻击状态
    }

    public void OnExit()
    {
        shady.Ani.SetBool("Attack", false);
        shady.isAttack = false; // 重置攻击状态
    }

    public EnemyState OnUpdate()
    {
        if (!shady.isAttack)
        {
            // 如果不能攻击，返回到移动状态
            return EnemyState.Move;
        }
        return state;
    }
}
