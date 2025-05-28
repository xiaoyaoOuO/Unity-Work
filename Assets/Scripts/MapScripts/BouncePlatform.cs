using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class BouncePlatform : MonoBehaviour
{
    [Header("路径点")]
    public Vector3 offset = new Vector3(0, 5, 0);

    [Header("参数设置")]
    public float delayBeforeMove = 1f;
    public float moveDuration = 0.5f;
    public float stayDuration = 1f;
    public float returnDuration = 0.8f;

    [Header("玩家 Tag")]
    public string playerTag = "Player";

    private Vector3 startPos;
    private Vector3 endPos;
    private bool isMoving = false;
    private Rigidbody2D rb;
    private BoxCollider2D boxCol;

    private ISoundEffectController soundEffectController;
    private AudioSource bouncePlatformAudioSource;
    private AudioClip bouncePlatformAudioClip;
    void Start()
    {
        soundEffectController = Game.instance.sceneManager;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        startPos = transform.position;
        endPos = startPos + offset;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // 只在玩家踩上平台时触发一次移动序列
        if (!isMoving && col.collider.CompareTag(playerTag))
        {
            StartCoroutine(MoveSequence());
        }
    }

    IEnumerator MoveSequence()
    {
        isMoving = true;

        // 延迟
        yield return new WaitForSeconds(delayBeforeMove);

        // 播放音效
        PlayBouncePlatformSound();

        // 去程
        yield return StartCoroutine(MoveTo(endPos, moveDuration));

        // 停留
        yield return new WaitForSeconds(stayDuration);

        // 回程
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

        // 最终位置
        rb.MovePosition(target);
    }

    private void PlayBouncePlatformSound()
    {
        // 获取音频资源
        if (bouncePlatformAudioClip == null)
        {
            bouncePlatformAudioClip = soundEffectController.GetSoundClip(SoundType.BouncePlatform);
        }
        // 从对象池获取音频源
        if (soundEffectController != null && bouncePlatformAudioClip != null)
        {
            bouncePlatformAudioSource = soundEffectController.GetAudioSource();
            // 配置并播放
            if (bouncePlatformAudioSource != null)
            {
                bouncePlatformAudioSource.PlayOneShot(bouncePlatformAudioClip);
                StartCoroutine(ReleaseAfterPlayback());
            }
        }
    }

    private IEnumerator ReleaseAfterPlayback()
    {
        // 等待音频播放完成
        yield return new WaitForSeconds(bouncePlatformAudioClip.length);
        // 释放音频源
        if (bouncePlatformAudioSource != null)
        {
            soundEffectController.ReleaseAudioSource(bouncePlatformAudioSource);
            bouncePlatformAudioSource = null;
        }
    }
}
