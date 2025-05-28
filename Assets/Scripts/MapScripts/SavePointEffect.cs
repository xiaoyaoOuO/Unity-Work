using UnityEngine;

public class SavePointEffect : MonoBehaviour
{
    public SpriteRenderer glowE; // 蓝光E的Sprite Renderer
    public ParticleSystem glowParticles; // 粒子引用
    private float fadeSpeed = 1.5f; // 淡入速度，可调整

    private void Start()
    {
        if (glowE != null)
            glowE.color = new Color(1, 1, 1, 0);

        if (glowParticles != null)
            glowParticles.Stop(); // 初始停止粒子
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 触发淡入动画
            StartCoroutine(FadeInGlow());

            // 保存当前位置到Player
            Player player = other.GetComponent<Player>();
            if (player != null)
                player.SetCheckPoint(transform.position);
        }
    }

    private System.Collections.IEnumerator FadeInGlow()
    {
        // 启动粒子特效
        if (glowParticles != null)
            glowParticles.Play();

        float targetAlpha = 1f;
        while (glowE.color.a < targetAlpha)
        {
            float newAlpha = Mathf.MoveTowards(glowE.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            glowE.color = new Color(1, 1, 1, newAlpha);
            yield return null;
        }
    }
}