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

    // 存储需要控制的组件
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<Tilemap> tilemaps = new List<Tilemap>();
    private List<Collider2D> colliders = new List<Collider2D>();
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>(); // 新增粒子系统列表

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

        // 2. 收集并隐藏所有 Tilemap
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

        // 4. 新增：收集并关闭所有粒子系统
        foreach (var ps in hiddenRegion.GetComponentsInChildren<ParticleSystem>(true))
        {
            particleSystems.Add(ps);
            ps.gameObject.SetActive(false); // 直接禁用整个粒子物体
        }
    }

    public void Reveal()
    {
        Init();
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        // 启用碰撞
        foreach (var col in colliders)
            col.enabled = true;

        // 新增：启用所有粒子系统
        foreach (var ps in particleSystems)
        {
            ps.gameObject.SetActive(true);
            ps.Play(); // 开始播放粒子
        }

        // 原有淡入逻辑
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            foreach (var sr in spriteRenderers)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, t);

            foreach (var tm in tilemaps)
                tm.color = new Color(tm.color.r, tm.color.g, tm.color.b, t);

            yield return null;
        }

        // 确保最终状态
        foreach (var sr in spriteRenderers)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);

        foreach (var tm in tilemaps)
            tm.color = new Color(tm.color.r, tm.color.g, tm.color.b, 1f);
    }
}