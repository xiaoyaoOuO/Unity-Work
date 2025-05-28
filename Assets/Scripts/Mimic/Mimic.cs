using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : Enemy
{
    private Rigidbody2D rb;
    private Animator Ani;

    [Header("属性")]
    public int maxHP = 30;
    public int currentHP;

    [Header("巡逻参数")]
    public Transform leftPoint;
    public Transform rightPoint;
    public float patrolSpeed = 2f;
    [SerializeField] public bool movingRight = false;
    private bool isDead = false;

    [Header("对玩家属性")]
    public Transform player; // 玩家Transform
    public float chaseRange = 6; // 追击范围
    public float attackRange = 0f; // 攻击范围
    public float revealRange = 1f; // turn range
    public float hideRange = 8f; // hide range
    // Start is called before the first frame update

    private bool ishiding = true;

    [SerializeField] private int Hidetime = 5000;
    [SerializeField] private float distanceToPlayer1;
    private float lastAttackTime = 0f;
    public float attackCooldown = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Ani = GetComponentInChildren<Animator>();
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        distanceToPlayer1 = distanceToPlayer;
        if (distanceToPlayer< revealRange)
        {
            Ani.SetBool("Reveal",true);
            ishiding = false;
        }
        else
        {
            if(ishiding == false&& isDead == false)
            {    
                
                Ani.SetBool("toHide", false);
                if (Ani != null)
                {
                    Ani.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
                 }
                if (Hidetime == 0) {
                    Ani.SetBool("toHide", true);
                    Ani.SetBool("Reveal", false);
                    ishiding = true;
                    Hidetime = 1000;
                }
                if (distanceToPlayer <= attackRange)
                {
                    Debug.Log("攻击");
                    rb.velocity = Vector2.zero;
                    if (Time.time - lastAttackTime > attackCooldown)
                    {
                        Attack();
                        lastAttackTime = Time.time;
                    }
                }
                else if (distanceToPlayer <= chaseRange)
                {
                    Debug.Log("追逐");
                    ChasePlayer();
                }
                else
                {
                    Debug.Log("巡逻");
                    Patrol();

                    Hidetime--;
                }

                
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
    public new void Attack()
    {

        if (Ani != null)
            Ani.SetTrigger("Attack");
    }
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
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
    public override void OnHit(int damage = 1)
    {
        currentHP -= damage;
       
        if (Ani != null)
        {
            Ani.SetTrigger("Hit");
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
 
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public new void Die()
    {
        if (Ani != null)
            Ani.SetTrigger("Die");
        isDead = true;
        rb.velocity = Vector2.zero;
        // 死亡动画播放后销毁对象，可用协程延迟
        Destroy(gameObject, 1.5f);
    }
}
