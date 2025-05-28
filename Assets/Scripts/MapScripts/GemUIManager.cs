using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GemUIManager : MonoBehaviour
{
    public static GemUIManager Instance { get; private set; }

    [Header("UI 引用")]
    public Image gemIcon;
    public TextMeshProUGUI gemCountText;

    private int currentGemCount = 0;

    private void Awake()
    {
        // 单例模式
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // 初始化显示
        if (gemCountText != null)
            gemCountText.text = "x " + currentGemCount;
    }

    // 外部调用：增加宝石
    public void AddGem(int amount = 1)
    {
        // 更新数量显示
        currentGemCount += amount;
        if (gemCountText != null)
            gemCountText.text = "x " + currentGemCount;

    }
}
