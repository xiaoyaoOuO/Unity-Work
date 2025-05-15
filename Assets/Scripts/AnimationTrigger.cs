using UnityEngine;

public class AnimationTrigger :MonoBehaviour {
    public Player player; // 引用到 Player 脚本的实例
    public Mushroom mushroom1;
    public void OnAttackEnd() { // 攻击结束时调用的函数
        player.AnimationTrigger();
    }

    public void Attackhit() {
        mushroom1.OnHit(1);
    }
}