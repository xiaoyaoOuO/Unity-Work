using UnityEngine;
using System.Collections;

public class BouncePad2D : MonoBehaviour
{
    [Header("玩家弹跳参数")]
    public float bounceForce = 15f;          // 弹跳力大小
    public Vector2 bounceDirection = Vector2.up; // 弹跳方向

    [Header("弹板参数")]
    public float bounceHeight = 2f;     // 弹起高度
    public float bounceSpeed = 5f;      // 弹起速度
    public float resetSpeed = 2f;       // 复位速度

    private Vector2 originalPosition;   // 原始位置
    private bool isActive = false;      // 是否正在运行协程

    private ISoundEffectController soundEffectController;
    private AudioSource bouncePadAudioSource;
    private AudioClip bouncePadAudioClip;
    void Start()
    {
        originalPosition = transform.position;

        soundEffectController = Game.instance.sceneManager;
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

            // 播放音效
            // PlayBounceSound();
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

    private void PlayBounceSound()
    {
        // 获取音频资源
        if (bouncePadAudioClip == null)
        {
            bouncePadAudioClip = soundEffectController.GetSoundClip(SoundType.BouncePad);
        }

        // 从对象池获取音频源
        if (soundEffectController != null && bouncePadAudioClip != null)
        {
            bouncePadAudioSource = soundEffectController.GetAudioSource();

            // 配置并播放
            if (bouncePadAudioSource != null)
            {
                bouncePadAudioSource.PlayOneShot(bouncePadAudioClip);

                StartCoroutine(ReleaseAfterPlayback());
            }
        }
        else
        {
            Debug.LogWarning("Bounce sound controller or clip not available");
        }
    }

    private System.Collections.IEnumerator ReleaseAfterPlayback()
    {
        // 等待音频播放完成
        yield return new WaitForSeconds(bouncePadAudioClip.length);

        // 释放资源
        if (bouncePadAudioSource != null)
        {
            soundEffectController.ReleaseAudioSource(bouncePadAudioSource);
            bouncePadAudioSource = null;
        }
    }
}