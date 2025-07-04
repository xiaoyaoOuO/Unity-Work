// WinScreenUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class WinScreenUI : MonoBehaviour
{
    [Header("UI元素")]
    public CanvasGroup blackScreen; // 黑屏渐变层
    public GameObject winPanel; // 通关面板
    public TextMeshProUGUI winTitle; // 通关标题

    private GemUIManager gemManager;
    private Player player;

    private void Awake()
    {
        gemManager = GemUIManager.Instance;
        player = FindObjectOfType<Player>();

        // 确保开始时隐藏
        blackScreen.alpha = 0;
        winPanel.SetActive(false);
    }

    public void ShowWinScreen()
    {
        // 开始显示流程
        StartCoroutine(ShowWinRoutine());
    }

    private IEnumerator ShowWinRoutine()
    {
        // 渐入黑屏
        float duration = 1f;
        float elapsed = 0;
        while (elapsed < duration)
        {
            blackScreen.alpha = Mathf.Lerp(0, 1, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        blackScreen.alpha = 1;

        // 显示通关面板
        winPanel.SetActive(true);
        winTitle.text = "通关成功!";

        // 暂停游戏
        Time.timeScale = 0;
    }
}