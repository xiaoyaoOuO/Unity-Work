// GemCollectible.cs
using UnityEngine;

public class GemCollectible : MonoBehaviour
{
    public Animator animator;

    private bool isCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            // 1. UI更新
            GemUIManager.Instance.AddGem(1);

            // 2. 播放收集动画
            isCollected = true;
            animator.Play("GemCollectAnim");

            // 3. 销毁对象
            Destroy(gameObject, 1.75f);

            // 4. 重置玩家的冲刺次数
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.dashCount = player.maxDashCount;
            }
        }
    }
}