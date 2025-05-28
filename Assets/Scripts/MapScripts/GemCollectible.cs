// GemCollectible.cs
using UnityEngine;

public class GemCollectible : MonoBehaviour
{
    public Animator animator;

    private bool isCollected = false;

    private ISoundEffectController soundEffectController;
    private AudioSource gemCollectAudioSource;
    private AudioClip gemCollectAudioClip;

    private void Start()
    {
        soundEffectController = Game.instance.sceneManager;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            // 1. UI����
            GemUIManager.Instance.AddGem(1);

            // 2. �����ռ�����
            isCollected = true;
            animator.Play("GemCollectAnim");
            Destroy(gameObject, 1.75f); // ���ݶ���ʱ������
            other.GetComponent<Player>().RefillDash();

            // 3. �����ռ���Ч
            PlayGemCollectSound();

            // 4. ���ٶ���
            Destroy(gameObject, 0.94f);

            // 5. ������ҵĳ�̴���
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.dashCount = player.maxDashCount;
            }
        }
    }

    private void PlayGemCollectSound()
    {
        // ��ȡ��Ƶ��Դ
        if (gemCollectAudioClip == null)
        {
            gemCollectAudioClip = soundEffectController.GetSoundClip(SoundType.GemCollect);
        }

        // �Ӷ���ػ�ȡ��ƵԴ
        if (soundEffectController != null && gemCollectAudioClip != null)
        {
            gemCollectAudioSource = soundEffectController.GetAudioSource();

            // ���ò�����
            if (gemCollectAudioSource != null)
            {
                gemCollectAudioSource.PlayOneShot(gemCollectAudioClip);

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
        yield return new WaitForSeconds(gemCollectAudioClip.length);

        // �ͷ���Դ
        if (gemCollectAudioSource!= null)
        {
            soundEffectController.ReleaseAudioSource(gemCollectAudioSource);
            gemCollectAudioSource = null;
        }
    }
}