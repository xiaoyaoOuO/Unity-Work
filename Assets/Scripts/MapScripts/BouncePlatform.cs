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
    private bool isReturning = false; // ���ƽ̨�Ƿ��ڻس�
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

        // ������Ч
        PlayBouncePlatformSound();

        // ȥ��
        yield return StartCoroutine(MoveTo(endPos, moveDuration));

        // ͣ��
        yield return new WaitForSeconds(stayDuration);

        // �س̣��������س̡���ǣ���ִ���ƶ���������رձ��
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

            // ������ڻس̣����ƽ̨�·��Ƿ������
            if (isReturning)
            {
            }

            yield return null;
        }

    }

    private void CheckCrush()
    {
        // ���ƽ̨�·��Ƿ�����ң�ʹ��һ���ܱ��ľ��ο�
        float detectionHeight = 0.1f;
        // ���������е�ƽ̨����
        float platformWidth = boxCol.bounds.size.x;
        // ����������ģ���ƽ̨�·�������ƽ̨�ױ�
        Vector2 boxCenter = (Vector2)transform.position
                            + Vector2.down * (boxCol.bounds.extents.y + detectionHeight / 2f);
        Vector2 boxSize = new Vector2(platformWidth, detectionHeight);

        // ��ȡ�����ڸ������ڵ� Collider2D
        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(playerTag))
            {
                // ��ұ�ѹס������ OnHit 5 ��
                Player player = hit.GetComponent<Player>();
                if (player != null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        player.OnHit();
                    }
                }

                // �Ѿ�ѹ����Һ󣬽����������
                isReturning = false;
                break;
            }
        }
    }

    private void PlayBouncePlatformSound()
    {
        // ��ȡ��Ƶ��Դ
        if (bouncePlatformAudioClip == null)
        {
            bouncePlatformAudioClip = soundEffectController.GetSoundClip(SoundType.BouncePlatform);
        }
        // �Ӷ���ػ�ȡ��ƵԴ
        if (soundEffectController != null && bouncePlatformAudioClip != null)
        {
            bouncePlatformAudioSource = soundEffectController.GetAudioSource();
            // ���ò�����
            if (bouncePlatformAudioSource != null)
            {
                bouncePlatformAudioSource.PlayOneShot(bouncePlatformAudioClip);
                StartCoroutine(ReleaseAfterPlayback());
            }
        }
    }

    private IEnumerator ReleaseAfterPlayback()
    {
        // �ȴ���Ƶ�������
        yield return new WaitForSeconds(bouncePlatformAudioClip.length);
        // �ͷ���ƵԴ
        if (bouncePlatformAudioSource != null)
        {
            soundEffectController.ReleaseAudioSource(bouncePlatformAudioSource);
            bouncePlatformAudioSource = null;
        }
    }
}
