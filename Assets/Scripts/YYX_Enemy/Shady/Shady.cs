
using UnityEngine;

public class Shady : Enemy
{
    public Rigidbody2D rb;
    public Animator Ani;
    public LayerMask enemyLayer;
    public Collider2D attackCollider;
    public Collider2D shadyCollider;
    public float currentHP;
    public float moveSpeed = 2f; // 移动速度
    public float maxHP = 3f; // 最大生命值
    public bool isDead = false; // 是否死亡
    public bool isAppear = false; //显形
    public bool canUpdate = false;
    public bool isAttack = false; // 是否攻击
    public float chaseDistance = 5f; // 追击距离
    public bool canAttack { get { return PlayerInAttackRange() && isAttack == false; } }
    public Facing currentFacing = Facing.Right;
    public Transform player; // 玩家Transform
    public EnemyStateMachine<IEnemyState> enemyStateMachine;
    public IEnemyState defaultState;
    void Awake()
    {
        defaultState = new ShadyIdleState(this);
        enemyStateMachine = new EnemyStateMachine<IEnemyState>();
        enemyStateMachine.AddState(new ShadyMoveState(this));
        enemyStateMachine.AddState(new ShadyAttackState(this));
        enemyStateMachine.AddState(new ShadyDeadState(this));
        enemyStateMachine.AddState(defaultState);
    }
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        Ani = GetComponent<Animator>();
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform; // 获取玩家Transform
    }

    public void WakeUp()
    {
        if (isAppear)
        {
            return;
        }
        Collider2D[] collider2Ds = new Collider2D[20];
        shadyCollider.OverlapCollider(new ContactFilter2D().NoFilter(), collider2Ds);
        foreach (Collider2D collider2D in collider2Ds)
        {
            if (collider2D != null)
            {
                if (collider2D.gameObject.tag == "Player")
                {
                    Ani.SetBool("canUpdate", true);
                    Ani.SetBool("Appear", true);
                    isAppear = true;
                    return;
                }
            }
        }
    }
    public override void Update()
    {
        base.Update();
        WakeUp();
        if (canUpdate)
        {
            enemyStateMachine.Update();
        }
    }

    public override void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        currentFacing = currentFacing == Facing.Right ? Facing.Left : Facing.Right;
    }

    public bool PlayerInAttackRange()
    {
        Collider2D[] hitColliders = new Collider2D[10];
        attackCollider.OverlapCollider(new ContactFilter2D { layerMask = enemyLayer }, hitColliders);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider == null) return false;
            if (hitCollider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    public float FindPlayerDistance()
    {
        return Vector2.Distance(transform.position, player.position);
    }

    public void Chase()
    {
        Vector2 direction = player.position - transform.position;
        if (direction.x > 0.2f && currentFacing == Facing.Left)
        {
            Flip();
        }
        else if (direction.x < -0.2f && currentFacing == Facing.Right)
        {
            Flip();
        }
        rb.velocity = new Vector2(direction.normalized.x / Mathf.Abs(direction.normalized.x) * moveSpeed, 0);
    }

    public void Attack()
    {
        Collider2D[] hitColliders = new Collider2D[10];
        attackCollider.OverlapCollider(new ContactFilter2D { layerMask = enemyLayer }, hitColliders);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider == null) return;
            if (hitCollider.CompareTag("Player"))
            {
                hitCollider.GetComponent<Player>().OnHit();
            }
        }
    }

    public override void OnHit(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        IEnemyState DieState = enemyStateMachine.GetState(EnemyState.Dead);
        enemyStateMachine.ChangeState(DieState);
    }
}
