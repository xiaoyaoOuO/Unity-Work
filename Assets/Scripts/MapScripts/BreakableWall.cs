using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class BreakableWallAnimation : MonoBehaviour
{
    [Header("横向冲击最小速度阈值 (世界单位/秒)")]
    public float breakVelocity = 5f;

    [Header("破裂动画帧 列表 (按播放顺序)")]
    public Sprite[] breakSprites;

    [Header("每帧持续时间 (秒)")]
    public float frameDuration = 0.1f;

    [Header("玩家标签 (默认 Player)")]
    public string playerTag = "Player";

    [Header("破碎后要触发的区域淡入脚本")]
    public HiddenZone revealScript;

    // 内部状态标志
    bool isBreaking = false;

    Collider2D col2d;
    SpriteRenderer sr;

    void Awake()
    {
        col2d = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isBreaking) return;
        if (!col.collider.CompareTag(playerTag)) return;

        // 只要有一次侧面且水平速度足够，就触发破碎
        foreach (var cp in col.contacts)
        {
            if (Mathf.Abs(cp.normal.x) > 0.5f
                && Mathf.Abs(col.relativeVelocity.x) >= breakVelocity)
            {
                StartCoroutine(BreakRoutine());
                break;
            }
        }
    }

    IEnumerator BreakRoutine()
    {
        isBreaking = true;

        // 1. 立即禁用碰撞，让玩家通过
        col2d.enabled = false;

        // 2. 播放破裂帧动画
        if (breakSprites != null && breakSprites.Length > 0)
        {
            for (int i = 0; i < breakSprites.Length; i++)
            {
                sr.sprite = breakSprites[i];
                yield return new WaitForSeconds(frameDuration);
            }
        }

        // 3. 播放完毕后，触发隐藏区域渐显
        if (revealScript != null)
            revealScript.Reveal();

        // 4. 播放完毕后销毁墙体
        Destroy(gameObject);
    }
}
