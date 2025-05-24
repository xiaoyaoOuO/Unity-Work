using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public Player player; // 引用到 Player 脚本的实例
    public void OnAttackEnd()
    { // 攻击结束时调用的函数
        player.AnimationEndTrigger();
    }

    public void OnAttackHit()
    { // 攻击命中时调用的函数
        player.AnimationAttackTrigger();
    }

    public void OnAnimationEnd()
    { // 动画结束时调用的函数
        player.AnimationEndTrigger();
    }

    public void FirstOnAnimationTrigger()
    {
        player.FirstAnimationTrigger();
    }
}