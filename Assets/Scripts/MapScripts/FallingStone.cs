using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingStone : MonoBehaviour
{
    [Header("玩家触发时延迟（秒）")]
    public float collapseDelay = 0.5f;
    [Header("掉落后等待复原（秒），≤0 则不复原")]
    public float resetDelay = 3f;
    [Header("检测玩家的 Tag")]
    public string playerTag = "Player";

    private Rigidbody2D rb;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isTriggered = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // 初始设为 Kinematic，静止不受重力影响
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.freezeRotation = true;

        // 记录原始位置/旋转以便复原
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    // 只有当玩家进入“FallTrigger”时才会调用此方法
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag(playerTag))
        {
            isTriggered = true;
            StartCoroutine(CollapseAndRestore());
        }
    }

    private IEnumerator CollapseAndRestore()
    {
        // 等待玩家完全走进触发区
        yield return new WaitForSeconds(collapseDelay);

        // 1️.让它掉下来
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;

        // 2️.等待 resetDelay，然后复原
        if (resetDelay > 0f)
        {
            yield return new WaitForSeconds(resetDelay);

            // 停止物理运动，复位位置
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            transform.SetPositionAndRotation(originalPosition, originalRotation);

            // 重置标志，允许下次再次触发
            isTriggered = false;
        }
    }
}
