using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class HiddenZone : MonoBehaviour
{
    [Header("������������")]
    public GameObject hiddenRegion;

    [Header("����ʱ�� (��)")]
    public float fadeDuration = 1f;

    // �洢������Ҫ�������Ⱦ��
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<Tilemap> tilemaps = new List<Tilemap>();
    // �洢������Ҫ���õ� Collider2D������ TilemapCollider2D��
    private List<Collider2D> colliders = new List<Collider2D>();

    private bool isInitialized = false;

    void Start()
    {
        Init();  // һ��ʼ������
    }

    private void Init()
    {
        if (isInitialized) return;
        isInitialized = true;

        // 1. �ռ����������� SpriteRenderer
        foreach (var sr in hiddenRegion.GetComponentsInChildren<SpriteRenderer>(true))
        {
            var c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 0f);
            spriteRenderers.Add(sr);
        }

        // 2. �ռ����������� Tilemap��ͨ������ Tilemap.color��
        foreach (var tm in hiddenRegion.GetComponentsInChildren<Tilemap>(true))
        {
            var c = tm.color;
            tm.color = new Color(c.r, c.g, c.b, 0f);
            tilemaps.Add(tm);
        }

        // 3. �ռ����������� Collider2D
        foreach (var col in hiddenRegion.GetComponentsInChildren<Collider2D>(true))
        {
            col.enabled = false;
            colliders.Add(col);
        }
    }

    // �ⲿ���ã���ʼ����
    public void Reveal()
    {
        Init();  // ȷ���Ѿ�����һ������
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        // ��������ײ
        foreach (var col in colliders)
            col.enabled = true;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            // ���� SpriteRenderer alpha
            foreach (var sr in spriteRenderers)
            {
                var c = sr.color;
                c.a = t;
                sr.color = c;
            }
            // ���� Tilemap alpha
            foreach (var tm in tilemaps)
            {
                var c = tm.color;
                c.a = t;
                tm.color = c;
            }

            yield return null;
        }

        // ȷ����ȫ��͸��
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
