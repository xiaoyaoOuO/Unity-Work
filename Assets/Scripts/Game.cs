using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class GameData
{
    public Vector3 CheckPointPosition; // 检查点位置
    public int PlayerHealth; // 玩家生命值
}


public class Game : MonoBehaviour
{
    public enum GameState
    { // 游戏状态枚举
        MainMenu,
        Playing,
        GameOver,
        Paused,
        BulletTime,
    }

    public static Game instance;

    public CameraManager cameraManager;
    public SceneManager sceneManager;
    public SaveManager saveManager;

    public void Awake()
    { // 在游戏开始时调用
        if (instance == null)
        { // 如果实例为空
            instance = this; // 将当前实例赋值给 instance
        }
        else
        { // 如果实例不为空
            Destroy(gameObject); // 销毁当前游戏对象
        }
    }

    public void Start()
    { // 在游戏开始时调用
        defaultFixedDeltaTime = Time.fixedDeltaTime; // 保存默认的固定时间步长
        saveManager = new SaveManager(); // 创建 SaveManager 实例
        this.StartGame();
    }

    public void OnApplicationQuit()
    { // 在应用程序退出时调用
        Debug.Log("Application is quitting, saving game data..."); // 输出日志
        EndGame();
    }

    public static GameState currentState = GameState.Playing; // 当前游戏状态
    public static GameState previousState; // 上一个游戏状态

    public static float defaultFixedDeltaTime;

    public void StartGame()
    { // 开始游戏
        currentState = GameState.Playing; // 设置当前游戏状态为 Playing
        Time.timeScale = 1; // 恢复游戏时间
        saveManager.LoadGameData();
    }
    public void EndGame()
    { // 结束游戏
        currentState = GameState.GameOver; // 设置当前游戏状态为 GameOver
        saveManager.SaveGameData();
    }

    public static void PauseGame()
    { // 暂停游戏
        previousState = currentState; // 保存上一个游戏状态
        currentState = GameState.Paused; // 设置当前游戏状态为 Paused
        Time.timeScale = 0; // 设置游戏时间为 0，即暂停
    }

    public IEnumerator Freeze(float time)
    { // 冻结游戏
        Time.timeScale = 0; // 设置游戏时间为 0，即暂停
        while (time > 0)
        { // 当时间大于 0 时
            time -= Time.unscaledDeltaTime; // 减去未缩放的时间增量
            yield return null; // 等待下一帧
        }
        Time.timeScale = 1; // 设置游戏时间为 1，即恢复
    }

    public void SaveGame()
    { // 保存游戏
        saveManager.SaveGameData();
    }

    public void LoadGame()
    {
        saveManager.LoadGameData();
    }

    [ContextMenu("Delete Game")]
    public void DeleteGame()
    {
        SaveManager.DeleteGame(); // 删除游戏数据
        Debug.Log("Game data deleted successfully."); // 输出日志
    }
}