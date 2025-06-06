using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    private Rigidbody2D rb;
    private Animator Ani;
    private bool isDashing = false;
    [SerializeField] private int count = 0;
    public float chaseRange = 7; // ׷����Χ
    public float attackRange = 2f; // ������Χ
    public Transform player; // ���Transform
    private float lastAttackTime = 0f;
    public float attackCooldown = 1f;

    [Header("����")]
    public int maxHP = 30;
    public int currentHP;

    [Header("Ѳ�߲���")]
    public Transform leftPoint;
    public Transform rightPoint;
    public float patrolSpeed = 2f;
    [SerializeField] public bool movingRight = false;
    
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Ani = GetComponentInChildren<Animator>();
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing) return; // ���ʱ���������ƶ�����
        Ani.SetBool("Hit", false);
        Ani.SetBool("Attack", false);
        // ����ʱ��Ѳ��
        if (!isDead && count == 0)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                rb.velocity = Vector2.zero;
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
            else if (distanceToPlayer <= chaseRange)
            {
                ChasePlayer();
            }
            else
            {
                Patrol();
            }

        }
        else { count--; }
        // �ƶ���������
        if (Ani != null)
        {
            Ani.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        }
    }
    void ChasePlayer()
    {
        if (player == null) return;
        float direction = player.position.x - transform.position.x;
        if (Mathf.Abs(direction) > 0.1f)
        {
            rb.velocity = new Vector2(Mathf.Sign(direction) * patrolSpeed, rb.velocity.y);
            if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
            {
                movingRight = !movingRight;
                Flip();
            }
        }
    }
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Patrol()
    {
        if (movingRight)
        {
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
            if (transform.position.x >= rightPoint.position.x)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);
            if (transform.position.x <= leftPoint.position.x)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    public new void Attack()
    {
        if (Ani != null)
            Ani.SetTrigger("Attack");
    }

    public new void Die()
    {
        if (Ani != null)
            Ani.SetTrigger("Die");
        isDead = true;
        rb.velocity = Vector2.zero;
        // �����������ź����ٶ��󣬿���Э���ӳ�
        Destroy(gameObject, 1.5f);
    }

    public override void OnHit(int damage = 1)
    {
        currentHP -= damage;
        if (Ani != null)
        {
            Ani.SetTrigger("Hit");
            rb.velocity = new Vector2(0, rb.velocity.y);
            count = 100;
        }
        OnhitFX(); // 调用受击特效函数
        if (currentHP <= 0)
        {
            Die();
        }
    }
}
