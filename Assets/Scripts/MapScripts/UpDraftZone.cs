using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class UpDraftZone : MonoBehaviour
{
    [Header("悬浮目标速度（单位：单位/秒）")]
    public float targetUpSpeed = 5f;
    [Header("速度达到目标所需时间")]
    public float smoothTime = 0.3f;

    private float currentVelocityY;             // 内部跟踪
    private float originalGravityScale;         // 备份玩家原重力
    private bool inZone = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!inZone && other.attachedRigidbody != null && other.CompareTag("Player"))
        {
            inZone = true;
            var rb = other.attachedRigidbody;
            // 备份并置零重力
            originalGravityScale = rb.gravityScale;
            rb.gravityScale = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var rb = other.attachedRigidbody;
        if (inZone && rb != null && other.CompareTag("Player"))
        {
            // 读取当前速度
            Vector2 v = rb.velocity;
            // 插值 y 速度到目标
            float newY = Mathf.SmoothDamp(v.y, targetUpSpeed, ref currentVelocityY, smoothTime);
            rb.velocity = new Vector2(v.x, newY);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (inZone && other.attachedRigidbody != null && other.CompareTag("Player"))
        {
            var rb = other.attachedRigidbody;
            // 恢复原重力
            rb.gravityScale = originalGravityScale;
            // 重置状态
            inZone = false;
            currentVelocityY = 0f;
        }
    }
}
