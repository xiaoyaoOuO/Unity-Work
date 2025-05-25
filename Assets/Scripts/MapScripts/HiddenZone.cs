using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class HiddenZone : MonoBehaviour
{
    [Header("隐藏区域父物体")]
    public GameObject hiddenRegion;

    [Header("淡入时长 (秒)")]
    public float fadeDuration = 1f;

    // 存储所有需要淡入的渲染器
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<Tilemap> tilemaps = new List<Tilemap>();
    // 存储所有需要启用的 Collider2D（包括 TilemapCollider2D）
    private List<Collider2D> colliders = new List<Collider2D>();

    private bool isInitialized = false;

    void Start()
    {
        Init();  // 一开始就隐藏
    }

    private void Init()
    {
        if (isInitialized) return;
        isInitialized = true;

        // 1. 收集并隐藏所有 SpriteRenderer
        foreach (var sr in hiddenRegion.GetComponentsInChildren<SpriteRenderer>(true))
        {
            var c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 0f);
            spriteRenderers.Add(sr);
        }

        // 2. 收集并隐藏所有 Tilemap（通过设置 Tilemap.color）
        foreach (var tm in hiddenRegion.GetComponentsInChildren<Tilemap>(true))
        {
            var c = tm.color;
            tm.color = new Color(c.r, c.g, c.b, 0f);
            tilemaps.Add(tm);
        }

        // 3. 收集并禁用所有 Collider2D
        foreach (var col in hiddenRegion.GetComponentsInChildren<Collider2D>(true))
        {
            col.enabled = false;
            colliders.Add(col);
        }
    }

    // 外部调用：开始渐显
    public void Reveal()
    {
        Init();  // 确保已经做过一次隐藏
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        // 先启用碰撞
        foreach (var col in colliders)
            col.enabled = true;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            // 更新 SpriteRenderer alpha
            foreach (var sr in spriteRenderers)
            {
                var c = sr.color;
                c.a = t;
                sr.color = c;
            }
            // 更新 Tilemap alpha
            foreach (var tm in tilemaps)
            {
                var c = tm.color;
                c.a = t;
                tm.color = c;
            }

            yield return null;
        }

        // 确保完全不透明
        foreach (var sr in spriteRenderers)
        {
            var c = sr.color;
            c.a = 1f;
            sr.color = c;
        }
        foreach (var tm in tilemaps)
        {
            var c = tm.color;
            c.a = 1f;
            tm.color = c;
        }
    }
}
