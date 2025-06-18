using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DeadScreenController : MonoBehaviour
{
    Button restartButton;
    Button quitButton;
    void Start()
    {
        restartButton = GetComponentInChildren<Button>();
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        quitButton = GetComponentsInChildren<Button>().FirstOrDefault(b => b.name == "QuitButton");
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnRestartButtonClicked()
    {
        // Restart the game
        UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        // 重新加载当前场景
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.name);
    }

    void OnQuitButtonClicked()
    {
        // Quit the game
        Application.Quit();
    }
}
