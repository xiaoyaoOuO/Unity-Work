using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aniScipt : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public Mushroom mushroom; // 在Inspector里拖拽父物体Mushroom脚本进来

        // 动画事件调用
     public void AttackMove()
     {

        mushroom.AttackMove();
  
     }
    

}
