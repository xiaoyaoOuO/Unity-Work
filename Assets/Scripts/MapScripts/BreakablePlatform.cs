using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(Collider2D), typeof(SpriteRenderer))]
public class BreakablePlatform : MonoBehaviour
{
    [Header("平台重现时间")]
    public float respawnTime = 3f;     // 重生时间

    // 组件引用
    private Animator anim;
    private Collider2D platformCollider;
    private SpriteRenderer spriteRenderer;
    private bool isActive = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        platformCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Player"))
        {
            // 仅当玩家从上方向下碰撞时触发
            if (collision.relativeVelocity.y <= 0)
            {
                StartBreak();
            }
        }
    }

    void StartBreak()
    {
        isActive = false;
        anim.SetTrigger("Break"); // 触发破裂动画
    }

    // 由动画事件在最后一帧调用
    public void OnBreakComplete()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        // 禁用碰撞和渲染
        platformCollider.enabled = false;
        spriteRenderer.enabled = false;

        // 等待重生时间
        yield return new WaitForSeconds(respawnTime);

        // 重置状态
        platformCollider.enabled = true;
        spriteRenderer.enabled = true;
        anim.Play("Idle", 0, 0f); // 回到初始状态
        isActive = true;
    }
}