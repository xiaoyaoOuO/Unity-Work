using UnityEngine;
using System.Collections;

public class BouncePad2D : MonoBehaviour
{
    [Header("弹跳参数")]
    public float bounceForce = 15f;          // 弹跳力大小
    public Vector2 bounceDirection = Vector2.up; // 弹跳方向

    [Header("弹板参数")]
    public float bounceHeight = 2f;     // 弹起高度
    public float bounceSpeed = 5f;      // 弹起速度
    public float resetSpeed = 2f;       // 复位速度

    private Vector2 originalPosition;   // 原始位置
    private bool isActive = false;      // 是否正在运行协程

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (otherRb != null && !isActive)
        {
            // 施加弹跳力
            Vector2 force = transform.TransformDirection(bounceDirection).normalized * bounceForce;
            otherRb.AddForce(force, ForceMode2D.Impulse);

            // 启动弹板动画协程
            StartCoroutine(BounceAnimation());
        }
    }

    IEnumerator BounceAnimation()
    {
        isActive = true;

        // 弹起阶段
        Vector2 targetUp = originalPosition + Vector2.up * bounceHeight;
        while (Vector2.Distance(transform.position, targetUp) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetUp, bounceSpeed * Time.deltaTime);
            yield return null;
        }

        // 复位阶段
        while (Vector2.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, originalPosition, resetSpeed * Time.deltaTime);
            yield return null;
        }

        // 确保精准复位
        transform.position = originalPosition;
        isActive = false;
    }
}