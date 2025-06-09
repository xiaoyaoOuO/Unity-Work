using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    Button beginButton;
    Button exitButton;
    Image blackImage;
    Image menu;

    void Start()
    {
        menu = transform.Find("Menu").GetComponent<Image>();
        beginButton = menu.transform.Find("Begin").GetComponent<Button>();
        exitButton = menu.transform.Find("Exit").GetComponent<Button>();
        blackImage = transform.Find("BlackImage").GetComponent<Image>();

        beginButton.onClick.AddListener(BeginGame);
        exitButton.onClick.AddListener(ExitGame);


        Debug.Log(beginButton);
    }

    void BeginGame()
    {
        Debug.Log("BeginGame");
        LoadGame();
    }

    void ExitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit(); // 退出游戏
    }

    void LoadGame()
    {
        StartCoroutine(Screenin());
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Scene 1");
    }

    IEnumerator Screenin()
    {
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            blackImage.color = Color.Lerp(Color.clear, Color.black, t); // 逐渐显示黑色
            yield return null;
        }
    }
}
