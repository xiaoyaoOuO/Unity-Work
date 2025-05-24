using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    void ChasePlayer()
    {
    }

    void Flip()
    {

    }

    // �ܻ�����
    public virtual void OnHit(int damage = 1)
    {

    }

    // �������������ڶ����¼��е��ã�
    public void Attack()
    {

    }

    public void AttackMove()
    {
    }


    // ��������
    public void Die()
    {
    }

    public virtual void OnhitFX()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red; // 设置颜色为红色
            StartCoroutine(ResetColor(spriteRenderer)); // 启动协程重置颜色
        }
    }
    
    private IEnumerator ResetColor(SpriteRenderer spriteRenderer)
    {
        yield return new WaitForSeconds(0.2f); // 等待0.1秒
        spriteRenderer.color = Color.white; // 重置颜色为白色
    }
}
