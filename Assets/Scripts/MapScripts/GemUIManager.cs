using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GemUIManager : MonoBehaviour
{
    public static GemUIManager Instance { get; private set; }

    [Header("UI ����")]
    public Image gemIcon;
    public TextMeshProUGUI gemCountText;

    private int currentGemCount = 0;

    private void Awake()
    {
        // ����ģʽ
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // ��ʼ����ʾ
        if (gemCountText != null)
            gemCountText.text = "x " + currentGemCount;
    }

    // �ⲿ���ã����ӱ�ʯ
    public void AddGem(int amount = 1)
    {
        // ����������ʾ
        currentGemCount += amount;
        if (gemCountText != null)
            gemCountText.text = "x " + currentGemCount;

    }
}
