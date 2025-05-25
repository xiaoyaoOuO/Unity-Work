using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class BouncePlatform : MonoBehaviour
{
    [Header("·����")]
    public Vector3 offset = new Vector3(0, 5, 0);

    [Header("��������")]
    public float delayBeforeMove = 1f;
    public float moveDuration = 0.5f;
    public float stayDuration = 1f;
    public float returnDuration = 0.8f;

    [Header("��� Tag")]
    public string playerTag = "Player";

    private Vector3 startPos;
    private Vector3 endPos;
    private bool isMoving = false;
    private Rigidbody2D rb;
    private BoxCollider2D boxCol;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        startPos = transform.position;
        endPos = startPos + offset;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // ֻ����Ҳ���ƽ̨ʱ����һ���ƶ�����
        if (!isMoving && col.collider.CompareTag(playerTag))
        {
            StartCoroutine(MoveSequence());
        }
    }

    IEnumerator MoveSequence()
    {
        isMoving = true;

        // �ӳ�
        yield return new WaitForSeconds(delayBeforeMove);

        // ȥ��
        yield return StartCoroutine(MoveTo(endPos, moveDuration));

        // ͣ��
        yield return new WaitForSeconds(stayDuration);

        // �س�
        yield return StartCoroutine(MoveTo(startPos, returnDuration));

        isMoving = false;
    }

    IEnumerator MoveTo(Vector3 target, float duration)
    {
        float elapsed = 0f;
        Vector3 initial = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            Vector3 newPos = Vector3.Lerp(initial, target, t);
            rb.MovePosition(newPos);
            yield return null;
        }

        // ����λ��
        rb.MovePosition(target);
    }
}
