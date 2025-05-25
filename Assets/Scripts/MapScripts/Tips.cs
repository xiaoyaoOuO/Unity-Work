using UnityEngine;
using UnityEngine.UI;

public class Tips : MonoBehaviour
{
    public CanvasGroup promptCanvasGroup;
    public float fadeDuration = 0.5f;
    public float displayTime = 5f;

    private Coroutine currentRoutine;
    private bool isFullyVisible; // 标记当前是否已完全显示

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
            // 如果已有协程在运行，先停止
            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            // 根据当前显示状态决定如何继续
            currentRoutine = StartCoroutine(HandlePrompt());
        }
    }

    private System.Collections.IEnumerator HandlePrompt()
    {
        // 如果未完全显示，则执行淡入
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

        // 重置显示计时（无论是否刚完成淡入）
        float remainingTime = displayTime;
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        // 淡出
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