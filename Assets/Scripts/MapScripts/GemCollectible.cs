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
            // 1. 增加数量
            GemUIManager.Instance.AddGem(1);

            // 2. 播放动画
            isCollected = true;
            animator.Play("GemCollectAnim");
            Destroy(gameObject, 1.75f); 
            other.GetComponent<Player>().RefillDash();

            // 3. 播放音效
            PlayGemCollectSound();

            // 4. 销毁物体
            Destroy(gameObject, 0.94f);

            // 5. 更新冲刺次数
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.dashCount = player.maxDashCount;
            }
        }
    }

    private void PlayGemCollectSound()
    {
        if (gemCollectAudioClip == null)
        {
            gemCollectAudioClip = soundEffectController.GetSoundClip(SoundType.GemCollect);
        }

        if (soundEffectController != null && gemCollectAudioClip != null)
        {
            gemCollectAudioSource = soundEffectController.GetAudioSource();

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
        yield return new WaitForSeconds(gemCollectAudioClip.length);

        if (gemCollectAudioSource!= null)
        {
            soundEffectController.ReleaseAudioSource(gemCollectAudioSource);
            gemCollectAudioSource = null;
        }
    }
}