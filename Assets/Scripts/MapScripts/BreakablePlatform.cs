using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(Collider2D), typeof(SpriteRenderer))]
public class BreakablePlatform : MonoBehaviour
{
    [Header("ƽ̨����ʱ��")]
    public float respawnTime = 3f;     // ����ʱ��

    // �������
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
            // ������Ҵ��Ϸ�������ײʱ����
            if (collision.relativeVelocity.y <= 0)
            {
                StartBreak();
            }
        }
    }

    void StartBreak()
    {
        isActive = false;
        anim.SetTrigger("Break"); // �������Ѷ���
    }

    // �ɶ����¼������һ֡����
    public void OnBreakComplete()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        // ������ײ����Ⱦ
        platformCollider.enabled = false;
        spriteRenderer.enabled = false;

        // �ȴ�����ʱ��
        yield return new WaitForSeconds(respawnTime);

        // ����״̬
        platformCollider.enabled = true;
        spriteRenderer.enabled = true;
        anim.Play("Idle", 0, 0f); // �ص���ʼ״̬
        isActive = true;
    }
}