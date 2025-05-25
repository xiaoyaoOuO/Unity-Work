using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class UpDraftZone : MonoBehaviour
{
    [Header("����Ŀ���ٶȣ���λ����λ/�룩")]
    public float targetUpSpeed = 5f;
    [Header("�ٶȴﵽĿ������ʱ��")]
    public float smoothTime = 0.3f;

    private float currentVelocityY;             // �ڲ�����
    private float originalGravityScale;         // �������ԭ����
    private bool inZone = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!inZone && other.attachedRigidbody != null && other.CompareTag("Player"))
        {
            inZone = true;
            var rb = other.attachedRigidbody;
            // ���ݲ���������
            originalGravityScale = rb.gravityScale;
            rb.gravityScale = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var rb = other.attachedRigidbody;
        if (inZone && rb != null && other.CompareTag("Player"))
        {
            // ��ȡ��ǰ�ٶ�
            Vector2 v = rb.velocity;
            // ��ֵ y �ٶȵ�Ŀ��
            float newY = Mathf.SmoothDamp(v.y, targetUpSpeed, ref currentVelocityY, smoothTime);
            rb.velocity = new Vector2(v.x, newY);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (inZone && other.attachedRigidbody != null && other.CompareTag("Player"))
        {
            var rb = other.attachedRigidbody;
            // �ָ�ԭ����
            rb.gravityScale = originalGravityScale;
            // ����״̬
            inZone = false;
            currentVelocityY = 0f;
        }
    }
}
