using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class BreakableWallAnimation : MonoBehaviour
{
    [Header("��������С�ٶ���ֵ (���絥λ/��)")]
    public float breakVelocity = 5f;

    [Header("���Ѷ���֡ �б� (������˳��)")]
    public Sprite[] breakSprites;

    [Header("ÿ֡����ʱ�� (��)")]
    public float frameDuration = 0.1f;

    [Header("��ұ�ǩ (Ĭ�� Player)")]
    public string playerTag = "Player";

    [Header("�����Ҫ������������ű�")]
    public HiddenZone revealScript;

    // �ڲ�״̬��־
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

        // ֻҪ��һ�β�����ˮƽ�ٶ��㹻���ʹ�������
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

        // 1. ����������ײ�������ͨ��
        col2d.enabled = false;

        // 2. ��������֡����
        if (breakSprites != null && breakSprites.Length > 0)
        {
            for (int i = 0; i < breakSprites.Length; i++)
            {
                sr.sprite = breakSprites[i];
                yield return new WaitForSeconds(frameDuration);
            }
        }

        // 3. ������Ϻ󣬴�������������
        if (revealScript != null)
            revealScript.Reveal();

        // 4. ������Ϻ�����ǽ��
        Destroy(gameObject);
    }
}
