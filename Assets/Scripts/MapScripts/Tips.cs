using UnityEngine;
using UnityEngine.UI;

public class Tips : MonoBehaviour
{
    public CanvasGroup promptCanvasGroup;
    public float fadeDuration = 0.5f;
    public float displayTime = 5f;

    private Coroutine currentRoutine;
    private bool isFullyVisible; // ��ǵ�ǰ�Ƿ�����ȫ��ʾ

    void Start()
    {
        if (promptCanvasGroup != null)
            promptCanvasGroup.alpha = 0;
        isFullyVisible = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �������Э�������У���ֹͣ
            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            // ���ݵ�ǰ��ʾ״̬������μ���
            currentRoutine = StartCoroutine(HandlePrompt());
        }
    }

    private System.Collections.IEnumerator HandlePrompt()
    {
        // ���δ��ȫ��ʾ����ִ�е���
        if (!isFullyVisible)
        {
            float timer = 0;
            while (timer < fadeDuration)
            {
                promptCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
                timer += Time.deltaTime;
                yield return null;
            }
            promptCanvasGroup.alpha = 1;
            isFullyVisible = true;
        }

        // ������ʾ��ʱ�������Ƿ����ɵ��룩
        float remainingTime = displayTime;
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        // ����
        float fadeTimer = 0;
        while (fadeTimer < fadeDuration)
        {
            promptCanvasGroup.alpha = Mathf.Lerp(1, 0, fadeTimer / fadeDuration);
            fadeTimer += Time.deltaTime;
            yield return null;
        }
        promptCanvasGroup.alpha = 0;
        isFullyVisible = false;
    }
}