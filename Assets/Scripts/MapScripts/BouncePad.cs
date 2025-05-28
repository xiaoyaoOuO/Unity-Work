using UnityEngine;
using System.Collections;

public class BouncePad2D : MonoBehaviour
{
    [Header("��ҵ�������")]
    public float bounceForce = 15f;          // ��������С
    public Vector2 bounceDirection = Vector2.up; // ��������

    [Header("�������")]
    public float bounceHeight = 2f;     // ����߶�
    public float bounceSpeed = 5f;      // �����ٶ�
    public float resetSpeed = 2f;       // ��λ�ٶ�

    private Vector2 originalPosition;   // ԭʼλ��
    private bool isActive = false;      // �Ƿ���������Э��

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
            // ʩ�ӵ�����
            Vector2 force = transform.TransformDirection(bounceDirection).normalized * bounceForce;
            otherRb.AddForce(force, ForceMode2D.Impulse);

            // �������嶯��Э��
            StartCoroutine(BounceAnimation());

            // ������Ч
            // PlayBounceSound();
        }
    }

    IEnumerator BounceAnimation()
    {
        isActive = true;

        // ����׶�
        Vector2 targetUp = originalPosition + Vector2.up * bounceHeight;
        while (Vector2.Distance(transform.position, targetUp) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetUp, bounceSpeed * Time.deltaTime);
            yield return null;
        }

        // ��λ�׶�
        while (Vector2.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, originalPosition, resetSpeed * Time.deltaTime);
            yield return null;
        }

        // ȷ����׼��λ
        transform.position = originalPosition;
        isActive = false;
    }

    private void PlayBounceSound()
    {
        // ��ȡ��Ƶ��Դ
        if (bouncePadAudioClip == null)
        {
            bouncePadAudioClip = soundEffectController.GetSoundClip(SoundType.BouncePad);
        }

        // �Ӷ���ػ�ȡ��ƵԴ
        if (soundEffectController != null && bouncePadAudioClip != null)
        {
            bouncePadAudioSource = soundEffectController.GetAudioSource();

            // ���ò�����
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
        // �ȴ���Ƶ�������
        yield return new WaitForSeconds(bouncePadAudioClip.length);

        // �ͷ���Դ
        if (bouncePadAudioSource != null)
        {
            soundEffectController.ReleaseAudioSource(bouncePadAudioSource);
            bouncePadAudioSource = null;
        }
    }
}