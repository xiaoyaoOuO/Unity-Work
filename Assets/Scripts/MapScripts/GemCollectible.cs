// GemCollectible.cs
using UnityEngine;

public class GemCollectible : MonoBehaviour
{
    public Animator animator;

    private bool isCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            // 1. UI����
            GemUIManager.Instance.AddGem(1);

            // 2. �����ռ�����
            isCollected = true;
            animator.Play("GemCollectAnim");

            // 3. ���ٶ���
            Destroy(gameObject, 1.75f);

            // 4. ������ҵĳ�̴���
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.dashCount = player.maxDashCount;
            }
        }
    }
}