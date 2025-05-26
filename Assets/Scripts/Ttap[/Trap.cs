using UnityEngine;

public class TrapAttack : Enemy
{
    private Animator Ani;

    public GameObject bulletPrefab; // 子弹预制体
    public Transform firePoint;     // 发射点
    public float attackRange = 10f; // 攻击距离
    public float attackInterval = 2f; // 攻击间隔
    private float timer = 0f;

    private Transform player;


    [SerializeField]private int currentHP;
    private int maxHP = 5;
    void Start()
    {
        Ani = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHP = maxHP;
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
        // 生成子弹

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        //bullet.layer = LayerMask.NameToLayer("Default");
        // 让子弹朝Player方向移动
        Vector3 dir = (player.position - firePoint.position).normalized;
        bullet.GetComponent<Bullet>().SetDirection(dir);
        
    }

    public override void OnHit(int damage = 1)
    {
        Debug.Log("enemy撞");
        currentHP -= damage;
        if (Ani != null)
        {
            Ani.SetTrigger("Hit");
        }
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public new void Die()
    {    
        Destroy(gameObject, 1.5f);
    }
}