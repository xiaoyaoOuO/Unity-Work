using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class BreakableWallAnimation : MonoBehaviour
{
    [Header("��������С�ٶ���ֵ (���絥λ/��)")]
    public float breakVelocity = 5f;

    [Header("���Ѷ���֡ �б� (������˳��)")]
    public Sprite[] breakSprites;

    [Header("ÿ֡����ʱ�� (��)")]
    public float frameDuration = 0.1f;

    [Header("��ұ�ǩ (Ĭ�� Player)")]
    public string playerTag = "Player";

    [Header("�����Ҫ������������ű�")]
    public HiddenZone revealScript;

    // �ڲ�״̬��־
    bool isBreaking = false;

    Collider2D col2d;
    SpriteRenderer sr;

    private ISoundEffectController soundEffectController;
    private AudioSource breakAudioSource;
    private AudioClip breakAudioClip;
    void Start()
    {
        soundEffectController = Game.instance.sceneManager;
    }

    void Awake()
    {
        col2d = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isBreaking) return;
        if (!col.collider.CompareTag(playerTag)) return;

        // ֻҪ��һ�β�����ˮƽ�ٶ��㹻���ʹ�������
        foreach (var cp in col.contacts)
        {
            if (Mathf.Abs(cp.normal.x) > 0.5f
                && Mathf.Abs(col.relativeVelocity.x) >= breakVelocity)
            {
                StartCoroutine(BreakRoutine());
                break;
            }
        }
    }

    IEnumerator BreakRoutine()
    {
        isBreaking = true;

        // 1. ����������ײ�������ͨ��
        col2d.enabled = false;

        // 2. ����������Ч
        PlayBreakSound();

        // 3. ��������֡����
        if (breakSprites != null && breakSprites.Length > 0)
        {
            for (int i = 0; i < breakSprites.Length; i++)
            {
                sr.sprite = breakSprites[i];
                yield return new WaitForSeconds(frameDuration);
            }
        }

        // 4. ������Ϻ󣬴�������������
        if (revealScript != null)
            revealScript.Reveal();

        // 5. ������Ϻ�����ǽ��
        Destroy(gameObject);
    }

    private void PlayBreakSound()
    {
        // ��ȡ��Ƶ��Դ
        if (breakAudioClip == null)
        {
            breakAudioClip = soundEffectController.GetSoundClip(SoundType.Break);
        }
        // �Ӷ���ػ�ȡ��ƵԴ
        if (soundEffectController != null && breakAudioClip != null)
        {
            breakAudioSource = soundEffectController.GetAudioSource();
            // ���ò�����
            if (breakAudioSource != null)
            {
                breakAudioSource.PlayOneShot(breakAudioClip);
                StartCoroutine(ReleaseAfterPlayback());
            }
        }
    }

    private IEnumerator ReleaseAfterPlayback()
    {
        // �ȴ���Ƶ�������
        yield return new WaitForSeconds(breakAudioClip.length);
        // �ͷ���ƵԴ�ض����
        soundEffectController.ReleaseAudioSource(breakAudioSource);
        breakAudioSource = null;
    }
}
