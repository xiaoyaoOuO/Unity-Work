using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class BouncePlatform : MonoBehaviour
{
    [Header("目标位置")]
    public Vector3 offset = new Vector3(0, 5, 0);

    [Header("移动参数")]
    public float delayBeforeMove = 1f;
    public float moveDuration = 0.5f;
    public float stayDuration = 1f;
    public float returnDuration = 0.8f;

    [Header("玩家Tag")]
    public string playerTag = "Player";

    private Vector3 startPos;
    private Vector3 endPos;
    private bool isMoving = false;
    private bool isReturning = false;
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
        if (!isMoving && col.collider.CompareTag(playerTag))
        {
            StartCoroutine(MoveSequence());
        }
    }

    IEnumerator MoveSequence()
    {
        isMoving = true;

        // 延时后触发
        yield return new WaitForSeconds(delayBeforeMove);

        // 播放音效
        PlayBouncePlatformSound();

        // 开始移动
        yield return StartCoroutine(MoveTo(endPos, moveDuration));

        // 停留一段时间
        yield return new WaitForSeconds(stayDuration);

        // 返回起始位置
        isReturning = true;
        yield return StartCoroutine(MoveTo(startPos, returnDuration));
        isReturning = false;

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

            if (isReturning)
            {
                // 检查是否有玩家被压在平台下方
                CheckCrush();
            }

            yield return null;
        }

    }

    private void CheckCrush()
    {
        float detectionHeight = 0.1f;
        float platformWidth = boxCol.bounds.size.x;
        
        Vector2 boxCenter = (Vector2)transform.position
                            + Vector2.down * (boxCol.bounds.extents.y + detectionHeight / 2f);
        Vector2 boxSize = new Vector2(platformWidth, detectionHeight);

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(playerTag))
            {
                // 找到玩家并触发击中效果
                Player player = hit.GetComponent<Player>();
                if (player != null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        player.OnHit();
                    }
                }

                // 停止返回
                isReturning = false;
                break;
            }
        }
    }

    private void PlayBouncePlatformSound()
    {
        if (bouncePlatformAudioClip == null)
        {
            bouncePlatformAudioClip = soundEffectController.GetSoundClip(SoundType.BouncePlatform);
        }

        if (soundEffectController != null && bouncePlatformAudioClip != null)
        {
            bouncePlatformAudioSource = soundEffectController.GetAudioSource();

            if (bouncePlatformAudioSource != null)
            {
                bouncePlatformAudioSource.PlayOneShot(bouncePlatformAudioClip);
                StartCoroutine(ReleaseAfterPlayback());
            }
        }
    }

    private IEnumerator ReleaseAfterPlayback()
    {

        yield return new WaitForSeconds(bouncePlatformAudioClip.length);

        if (bouncePlatformAudioSource != null)
        {
            soundEffectController.ReleaseAudioSource(bouncePlatformAudioSource);
            bouncePlatformAudioSource = null;
        }
    }
}
