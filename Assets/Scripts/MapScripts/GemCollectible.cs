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
            // 1. UI更新
            GemUIManager.Instance.AddGem(1);

            // 2. 播放收集动画
            isCollected = true;
            animator.Play("GemCollectAnim");

            // 3. 播放收集音效
            PlayGemCollectSound();

            // 4. 销毁对象
            Destroy(gameObject, 0.94f);

            // 5. 重置玩家的冲刺次数
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.dashCount = player.maxDashCount;
            }
        }
    }

    private void PlayGemCollectSound()
    {
        // 获取音频资源
        if (gemCollectAudioClip == null)
        {
            gemCollectAudioClip = soundEffectController.GetSoundClip(SoundType.GemCollect);
        }

        // 从对象池获取音频源
        if (soundEffectController != null && gemCollectAudioClip != null)
        {
            gemCollectAudioSource = soundEffectController.GetAudioSource();

            // 配置并播放
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
        // 等待音频播放完成
        yield return new WaitForSeconds(gemCollectAudioClip.length);

        // 释放资源
        if (gemCollectAudioSource!= null)
        {
            soundEffectController.ReleaseAudioSource(gemCollectAudioSource);
            gemCollectAudioSource = null;
        }
    }
}