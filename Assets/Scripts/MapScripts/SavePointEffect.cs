using UnityEngine;

public class SavePointEffect : MonoBehaviour
{
    public SpriteRenderer glowE; // ����E��Sprite Renderer
    public ParticleSystem glowParticles; // ��������
    private float fadeSpeed = 1.5f; // �����ٶȣ��ɵ���

    private void Start()
    {
        if (glowE != null)
            glowE.color = new Color(1, 1, 1, 0);

        if (glowParticles != null)
            glowParticles.Stop(); // ��ʼֹͣ����
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �������붯��
            StartCoroutine(FadeInGlow());

            // ���浱ǰλ�õ�Player
            Player player = other.GetComponent<Player>();
            if (player != null)
                player.SetCheckPoint(transform.position);
        }
    }

    private System.Collections.IEnumerator FadeInGlow()
    {
        // ����������Ч
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