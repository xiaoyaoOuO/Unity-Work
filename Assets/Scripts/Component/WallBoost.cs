using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBoost
{
    public float timer;

    public void Update()
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
