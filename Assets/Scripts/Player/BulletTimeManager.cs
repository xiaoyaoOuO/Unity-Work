
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class BulletTimeManager :MonoBehaviour
{
    public float defaultFixedDeltaTime; // 默认的固定时间间隔
    public float BulletTimeScale; // 子弹时间的缩放比例
    public bool isBulletTime; // 子弹时间标志，0 表示非子弹时间，1 表示子弹时间
    public float prevTimeScale;
    public static BulletTimeManager instance; // 单例实例
    public Image ScreenImage; // 屏幕渲染的图像

    private void Awake() { // 单例模式，确保只有一个实例存在
        if (instance == null) { // 如果实例不存在，将当前实例赋值给 instance 变量
            instance = this; // 将当前实例赋值给 instance 变量
        } else { // 如果实例已经存在，销毁当前游戏对象
            Destroy(gameObject); // 销毁当前游戏对象
        }
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        BulletTimeScale = 0.3f; // 设置子弹时间的缩放比例
    }

    public void Enter() { // 子弹时间
        prevTimeScale = Time.timeScale; // 记录当前时间缩放比例
        isBulletTime = true; 
        Time.timeScale = BulletTimeScale; 
        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
    }

    public void Exit() { // 正常时间
        isBulletTime = false;
        Time.timeScale = prevTimeScale; // 恢复之前的时间缩放比例
        Time.fixedDeltaTime = defaultFixedDeltaTime;
    }

    public void Update() {
        if (isBulletTime) { // 如果是子弹时间，显示屏幕图像
            Color color = ScreenImage.color; // 获取屏幕图像的颜色
            color.a = Mathf.MoveTowards(color.a, 0.3f, 2*Time.deltaTime); // 逐渐增加透明度
            ScreenImage.color = color; 

            Time.timeScale = Mathf.MoveTowards(Time.timeScale, BulletTimeScale, Time.unscaledDeltaTime); // 逐渐增加时间缩放比例
        }else{
            Color color = ScreenImage.color; // 获取屏幕图像的颜色
            color.a = Mathf.MoveTowards(color.a, 0f, 2*Time.deltaTime); // 逐渐减少透明度
            ScreenImage.color = color; 
        }
    }

}