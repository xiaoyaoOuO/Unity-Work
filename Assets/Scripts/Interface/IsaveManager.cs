using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager
{
    public void SaveGameData(GameData gameData); // 保存游戏数据
    public void LoadGameData(GameData gameData); // 加载游戏数据
}
