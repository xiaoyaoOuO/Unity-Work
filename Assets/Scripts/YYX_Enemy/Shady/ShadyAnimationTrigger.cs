using UnityEngine;

public class ShadyAnimationTrigger : MonoBehaviour
{
    public Shady shady;

    void AttackFinished()
    {
        shady.isAttack = false; // 攻击结束后重置攻击状态
    }

    void DeadAnimationFinished()
    {
        shady.isDead = true; // 死亡动画结束后设置死亡状态
    }

    void AnimationAttack()
    {
        shady.Attack();
    }

    void ShadyAppear()
    {
        shady.Ani.SetBool("Appear", false);
        shady.enemyStateMachine.Initialize(shady.defaultState);
        shady.canUpdate = true;
    }
}
