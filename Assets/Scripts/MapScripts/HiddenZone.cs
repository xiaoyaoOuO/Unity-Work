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

    // �洢��Ҫ���Ƶ����
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<Tilemap> tilemaps = new List<Tilemap>();
    private List<Collider2D> colliders = new List<Collider2D>();
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>(); // ��������ϵͳ�б�

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

        // 2. �ռ����������� Tilemap
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

        // 4. �������ռ����ر���������ϵͳ
        foreach (var ps in hiddenRegion.GetComponentsInChildren<ParticleSystem>(true))
        {
            particleSystems.Add(ps);
            ps.gameObject.SetActive(false); // ֱ�ӽ���������������
        }
    }

    public void Reveal()
    {
        Init();
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        // ������ײ
        foreach (var col in colliders)
            col.enabled = true;

        // ������������������ϵͳ
        foreach (var ps in particleSystems)
        {
            ps.gameObject.SetActive(true);
            ps.Play(); // ��ʼ��������
        }

        // ԭ�е����߼�
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

        // ȷ������״̬
        foreach (var sr in spriteRenderers)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);

        foreach (var tm in tilemaps)
            tm.color = new Color(tm.color.r, tm.color.g, tm.color.b, 1f);
    }
}