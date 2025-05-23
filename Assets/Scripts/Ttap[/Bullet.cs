using UnityEngine;
using static Unity.UOS.COSXML.Model.Tag.RestoreConfigure;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // 子弹速度
    private Vector2 direction; // 子弹移动方向
    private float lifeTime = 2f; // 子弹生命周期（秒）
    private float timer = 0f;
    bool flipped;
    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }
    void Start()
    {

    }
    void Update()
    {
        // 子弹移动
        transform.Translate(direction * speed * Time.deltaTime);
        // 可选：子弹自转（绕Z轴）
        //  transform.Rotate(0, 0, 360 * Time.deltaTime);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("碰撞");
        if (this.gameObject.tag == "enemyAttack")//如果子弹标签是enemyAttack
        {
            if (other.CompareTag("Player"))
            {
                //health -- ;

                Destroy(this.gameObject);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                Destroy(this.gameObject);
            }

        }
        else if (this.gameObject.tag == "playerAttack")//如果子弹标签是playerAttack
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //health--;

                Destroy(this.gameObject);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Flip()
    {
        if (flipped) return;
        flipped = true;
       
        direction = -direction;

        this.gameObject.tag = "playerAttack";//改变标签

        //GameObject.Find("FramePause").GetComponent<FramePause>().BeHitPause(12);//渲染画面->顿帧->还原画面

        //GameObject.Find("Impulse").GetComponent<ImpulseTest>().Impulse();//震动屏幕
        
    }
}