using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager
{
    public List<ISaveManager> saveManagers;

    string filePath;

    public SaveManager()
    {
        saveManagers = getSaveManagers();
        filePath = System.IO.Directory.GetCurrentDirectory() + "/savefile.json";
    }
    
    public void SaveGameData()
    {
        saveManagers = getSaveManagers();
        GameData gameData = new GameData();
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveGameData(gameData);
        }
        // 将游戏数据序列化为JSON字符串
        string dataJson = JsonUtility.ToJson(gameData);
        // 将JSON字符串保存到文件
        try
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataJson);
                }
            }
            Debug.Log("Game data saved successfully to " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save game data: " + e.Message);
        }
    }

    public void LoadGameData()
    {
        if (!System.IO.File.Exists(filePath))
        {
            Debug.LogWarning("Save file not found at " + filePath);
            return; // 如果文件不存在，直接返回
        }
        // 从文件加载JSON字符串
        string dataJson = System.IO.File.ReadAllText(filePath);
        // 将JSON字符串反序列化为游戏数据对象
        GameData gameData = JsonUtility.FromJson<GameData>(dataJson);
        if (gameData == null)
        {
            Debug.LogError("Failed to load game data from " + filePath);
            return; // 如果反序列化失败，直接返回
        }
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadGameData(gameData);
        }
    }

    public List<ISaveManager> getSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }
}
