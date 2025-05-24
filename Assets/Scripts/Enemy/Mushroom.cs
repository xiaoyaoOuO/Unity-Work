using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
    private Rigidbody2D rb;
    private Animator Ani;
    [SerializeField] private int count=0;

    [Header("特效Prefab")]
    public GameObject hitVFX;      // 受击特效
    public GameObject deathVFX;    // 死亡特效
    public GameObject attackVFX;   // 攻击特效

    [Header("属性")]
    public int maxHP = 30;
    public int currentHP;

    [Header("巡逻参数")]
    public Transform leftPoint;
    public Transform rightPoint;
    public float patrolSpeed = 2f;
    [SerializeField] public bool movingRight = false;
    private bool isDead = false;

    public Transform player; // 玩家Transform
    public float chaseRange = 7; // 追击范围
    public float attackRange = 4f; // 攻击范围
    private bool isChasing = false;

    private float lastAttackTime = 0f;
    public float attackCooldown = 1f;

    private bool isDashing = false;

    private int stoping = 1000; 

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
        if (isDashing) return; // 冲刺时不做其他移动控制
        Ani.SetBool("Hit", false);
        Ani.SetBool("Attack", false);
        // 死亡时不巡逻
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
                stoping--;
                if (stoping != 0) { Patrol(); }
                else { 
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    stoping = 1000;
                    count = 1000;
                }

            }

        }
        else { count--; }
        // 移动动画控制
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

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // 受击函数
    public override void OnHit(int damage = 1)
    {
        currentHP -= damage;
        if (hitVFX != null)
            Instantiate(hitVFX, transform.position, Quaternion.identity);
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

    // 攻击函数（可在动画事件中调用）
    public new void Attack()
    {
        if (attackVFX != null)
            Instantiate(attackVFX, transform.position, Quaternion.identity);
        if (Ani != null)
            Ani.SetTrigger("Attack");
    }

    public new void AttackMove()
    {
        isDashing = true;
        float dashDistance = 2.0f; // 冲刺距离，可调整
        float dashTime = 0.2f;     // 冲刺时间，可调整
        Vector2 dashDir = movingRight ? Vector2.right : Vector2.left;
        rb.velocity = dashDir * (dashDistance / dashTime);
        StartCoroutine(ResetVelocityAfterDash(dashTime));
    }

    private IEnumerator ResetVelocityAfterDash(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector2.zero;
        isDashing = false;
    }

    // 死亡函数
    public new void Die()
    {
        if (deathVFX != null)
            Instantiate(deathVFX, transform.position, Quaternion.identity);
        if (Ani != null)
            Ani.SetTrigger("Die");
        isDead = true;
        rb.velocity = Vector2.zero;
        // 死亡动画播放后销毁对象，可用协程延迟
        Destroy(gameObject, 1.5f);
    }
}
