using UnityEngine;
using static Unity.UOS.COSXML.Model.Tag.RestoreConfigure;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // �ӵ��ٶ�
    private Vector2 direction; // �ӵ��ƶ�����
    private float lifeTime = 2f; // �ӵ��������ڣ��룩
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
        // �ӵ��ƶ�
        transform.Translate(direction * speed * Time.deltaTime);
        // ��ѡ���ӵ���ת����Z�ᣩ
        //  transform.Rotate(0, 0, 360 * Time.deltaTime);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("��ײ");
        if (this.gameObject.tag == "enemyAttack")//����ӵ���ǩ��enemyAttack
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
        else if (this.gameObject.tag == "playerAttack")//����ӵ���ǩ��playerAttack
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

        this.gameObject.tag = "playerAttack";//�ı��ǩ

        //GameObject.Find("FramePause").GetComponent<FramePause>().BeHitPause(12);//��Ⱦ����->��֡->��ԭ����

        //GameObject.Find("Impulse").GetComponent<ImpulseTest>().Impulse();//����Ļ
        
    }
}