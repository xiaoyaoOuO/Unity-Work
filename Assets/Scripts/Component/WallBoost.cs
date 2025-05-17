using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBoost
{
    public float timer;

    public void Update()        //TODO： 检测墙跳
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
            }
        }
    }
}
