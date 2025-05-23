using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBoost  //好像暂时没用
{
    public float timer;

    public void Update()        //TODO： 检测墙跳
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
}
