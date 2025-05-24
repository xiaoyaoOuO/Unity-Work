using UnityEngine;

public class TrapAttack : MonoBehaviour
{
    private Animator Ani;

    public GameObject bulletPrefab; // �ӵ�Ԥ����
    public Transform firePoint;     // �����
    public float attackRange = 10f; // ��������
    public float attackInterval = 2f; // �������
    private float timer = 0f;

    private Transform player;

    void Start()
    {
        Ani = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            timer += Time.deltaTime;
            if (timer >= attackInterval)
            {
                Attack();
                timer = 0f;
            }
        }
    }

    void Attack()
    {
        Ani.SetTrigger("Attack");
        // �����ӵ�

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        //bullet.layer = LayerMask.NameToLayer("Default");
        // ���ӵ���Player�����ƶ�
        Vector3 dir = (player.position - firePoint.position).normalized;
        bullet.GetComponent<Bullet>().SetDirection(dir);
        
    }
}