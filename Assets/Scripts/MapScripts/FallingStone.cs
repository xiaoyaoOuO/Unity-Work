using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingStone : MonoBehaviour
{
    [Header("玩家触发时延迟（秒）")]
    public float collapseDelay = 0.5f;
    [Header("掉落后等待复原（秒），≤0 则不复原")]
    public float resetDelay = 3f;
    [Header("检测玩家的 Tag")]
    public string playerTag = "Player";

    [Header("摇晃设置")]
    [Tooltip("摇晃持续时间")]
    public float shakeDuration = 0.8f;
    [Tooltip("摇晃强度")]
    public float shakeIntensity = 0.1f;
    [Tooltip("摇晃频率（每秒次数）")]
    public float shakeFrequency = 30f;

    private Rigidbody2D rb;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isTriggered = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.freezeRotation = true;

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag(playerTag))
        {
            isTriggered = true;
            StartCoroutine(ShakeBeforeFall());
        }
    }

    // 新增的摇晃协程
    private IEnumerator ShakeBeforeFall()
    {
        float elapsed = 0f;
        Vector3 basePosition = transform.position;

        // 摇晃阶段
        while (elapsed < shakeDuration)
        {
            float offsetX = Mathf.PerlinNoise(Time.time * shakeFrequency, 0) * 2 - 1;
            float offsetY = Mathf.PerlinNoise(0, Time.time * shakeFrequency) * 2 - 1;
            Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0) * shakeIntensity;

            transform.position = basePosition + shakeOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 回归原位后开始坠落流程
        transform.position = originalPosition;
        StartCoroutine(CollapseAndRestore());
    }

    private IEnumerator CollapseAndRestore()
    {
        // 保留原来的延迟逻辑
        yield return new WaitForSeconds(collapseDelay);

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;

        if (resetDelay > 0f)
        {
            yield return new WaitForSeconds(resetDelay);

            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            transform.SetPositionAndRotation(originalPosition, originalRotation);

            isTriggered = false;
        }
    }
}