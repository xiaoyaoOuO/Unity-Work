using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyDeadState : IEnemyState
{
    public EnemyState state => EnemyState.Dead;
    public string stateName => "Dead";
    public Shady shady;
    public ShadyDeadState(Shady shady)
    {
        this.shady = shady;
    }

    public void OnEnter()
    {
        shady.rb.velocity = Vector2.zero; // 停止移动
        shady.Ani.SetTrigger("Dead");
        shady.isDead = false; // 设置死亡状态为false，等待动画结束后再设置为true
    }

    public void OnExit()
    {
        shady.Ani.SetBool("Dead", false);
    }

    public EnemyState OnUpdate()
    {
        if (shady.isDead)
        {
            GameObject.Destroy(shady.gameObject); // 销毁敌人对象
        }
        return state;
    }
}
