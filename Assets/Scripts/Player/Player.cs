using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Facing{Left, Right}

public class Player : MonoBehaviour, ISaveManager
{
    #region Player Variables
    [Header("基础移动")]
    public float speed = 5f; // 玩家移动速度
    public float acceleration = 1f; // 移动加速度
    public float jumpForce = 3f; // 跳跃力度
    public float gravity = 0.5f; // 重力系数
    public Facing facing = Facing.Right;
    public float jumpGraceTime = 0.1f; // 跳跃宽容时间
    [Header("攻击")]
    public float attackCooldown = 1f; // 攻击冷却时间
    public float attackTimer;
    [Header("冲刺")]
    public float dashSpeed = 15f; // 冲刺速度
    public float upDashSpeed = 10f; // 向上冲刺速度
    public float dashDuration = 0.3f; // 冲刺持续时间 
    public Vector2 dashDirection; // 冲刺方向
    public int dashCount = 1; // 当前冲刺次数
    public int maxDashCount = 1; // 最大冲刺次数
    public float trailFXInterval = 0.05f; // 冲刺特效间隔
    [Header("子弹时间")]
    public float BulletTimeDuration = 0.5f; // 子弹时间持续时间
    public float BulletTimeRefillSpeed = 0.5f; // 时间回复速度
    public float BulletTimer = 0f; // 子弹时间计时
    public float BulletTimeCooldownTimer;
    [Header("翻滚")]
    public float rollSpeed = 15f; // 翻滚速度
    public float rollCooldown = 1f; // 翻滚冷却时间
    [Header("爬墙")]
    public float wallSlideSpeed = 2f; // 爬墙速度
    [Header("墙跳")]
    public float wallJumpForce = 10f; // 墙跳y轴力
    public float wallJumpDuration = 0.3f; // 墙跳持续时间
    public float wallJumpBoostSpeed = 10f; // 墙跳x轴力
    [Header("人物状态")]
    public int maxHealth;
    public bool canGrab { get { return GameInput.IsGrabPressed() && HeadWallCheck(); } } // 是否可以抓墙
    public bool canAttack { get { return attackTimer <= 0 && GameInput.IsAttackPressed(); } }
    public bool canDash { get { return dashCount > 0 && GameInput.IsDashPressed(); } }
    public bool canJump { get { return GameInput.IsJumpPressed() &&  jumpCheck.AllowJump() && !HaveWallAbove(); } }
    public bool canIntoBulletTime { get { return GameInput.IsBulletTimePressed() && BulletTimeCooldownTimer >= BulletTimeDuration; } }
    public bool canRoll { get { return GameInput.IsRollPressed() && IsGrounded(); } }
    public Collider2D UpAttackCollider; // Reference to the UpAttack collider
    public Collider2D DownAttackCollider; // Reference to the DownAttack collider
    public Collider2D RightAttackCollider; // Reference to the RightAttack collider
    public Collider2D HeadWallCheckCollider;
    public GameObject CorrectCrossWall;
    public LayerMask enemyLayer; // Layer mask for enemies
    public LayerMask wallLayer; // Layer mask for walls
    public LayerMask bulletLayer; // Layer mask for bullets

    [Header("存档")]
    private Vector3 CheckPointPosition; // 检查点位置

    #endregion

    public PlayerState playerState;
    private FiniteStateMachine<IState> fsm;

    //Unity组件
    public Animator animator;
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;  //人物的碰撞盒
    public Game_UI game_UI;
    public Image DeadScreen;

    public IEffectController effectController;
    public ICamera cameraManager;
    public ISoundEffectController soundEffectController;

    public SettingUI settingUI;

    public JumpCheck jumpCheck;

    // Start is called before the first frame update
    void Start()
    {
        fsm = new FiniteStateMachine<IState>(); // Initialize the finite state machine
        fsm.AddState(new PlayerIdleState(this));
        fsm.AddState(new PlayerRunState(this));
        fsm.AddState(new PlayerAirState(this));
        fsm.AddState(new PlayerDashState(this));
        fsm.AddState(new PlayerAttackState(this));
        fsm.AddState(new PlayerJumpState(this));
        fsm.AddState(new PlayerRunAttackState(this));
        fsm.AddState(new PlayerStandingAttackState(this));
        fsm.AddState(new PlayerDashAttackState(this));
        fsm.AddState(new PlayerRollState(this));
        fsm.AddState(new PlayerRollAttackState(this));
        fsm.AddState(new PlayerWallState(this));
        fsm.AddState(new PlayerWallJumpState(this));
        fsm.AddState(new PlayerDeadState(this));


        //获取组件
        animator = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();

        rb.gravityScale = gravity;

        //特效组
        effectController = Game.instance.sceneManager;
        cameraManager = Game.instance.cameraManager;
        soundEffectController = Game.instance.sceneManager;


        //跳跃检查器
        jumpCheck = new JumpCheck(this);

        fsm.Initialize();

        if (playerState == null)
        {
            playerState = new PlayerState(this, maxHealth); // Initialize player state with max health
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForSettingUI();
        // Update the state machine
        fsm.Update();

        //输入缓冲检测
        GameInput.Jump.Update();
        GameInput.Dash.Update();
        GameInput.Roll.Update();

        UpdatePlayerController();
        CheckForBulletTime(); // Check if the player can enter bullet time

        cameraManager.UpdateCameraPosition(this.transform.position);

        jumpCheck.Update();
    }

    private void CheckForSettingUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settingUI.gameObject.SetActive(!settingUI.gameObject.activeSelf);
            if (settingUI.gameObject.activeSelf)
            {
                Time.timeScale = 0f; // 暂停游戏
            }
            else
            {
                Time.timeScale = 1f; // 恢复游戏
            }
        }
    }

    private void UpdatePlayerController()
    {
        if (IsGrounded() && rb.velocity.y <= 0)
        {
            RefillDash();
        }

        if (attackTimer > 0)
        { // If the attack timer is greater than 0, decrement it{
            attackTimer -= Time.deltaTime; // Decrement the attack timer{ 
        }
    }

    public void SetAnimation(string animationName, bool value = true)
    {
        animator.SetBool(animationName, value);
    }

    public void SetAnimation(string animationName, float value)
    {
        animator.SetFloat(animationName, value);
    }

    public void SetAnimation(string animationName, int value)
    {
        animator.SetInteger(animationName, value);
    }

    public void SetVelocity(Vector2 velocity)
    {
        if (facing == Facing.Left && velocity.x > 0 || facing == Facing.Right && velocity.x < 0) // Check if the player is moving in the opposite direction{
        {
            OnFlip();
        }
        rb.velocity = velocity;
    }

    public void ZeroVelocity()
    {
        rb.velocity = Vector2.zero;
    }

    public void OnFlip()
    {
        facing = (facing == Facing.Right) ? Facing.Left : Facing.Right; // Flip the facing direction
        transform.Rotate(0f, 180f, 0f);
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + 0.1f, wallLayer);
        if (hit.collider == null)
        { // If the player is not grounded, return false
            return false;
        }
        return true;
    }

    public void Jump()
    {
        SetVelocity(new Vector2(rb.velocity.x, jumpForce));
    }

    public void SuperJump()
    {
        SetVelocity(new Vector2(rb.velocity.x, jumpForce) * 1.5f);
    }

    public Vector2 getVelocity() => rb.velocity;
    public void FirstAnimationTrigger() => fsm.CurrentState.FirstAnimationTrigger();
    public void AnimationEndTrigger() => fsm.CurrentState.AnimationEndTrigger();
    public void AnimationAttackTrigger() => fsm.CurrentState.AnimationAttackTrigger(); // Trigger the attack animation

    public void RefillDash()
    { // Refill the dash count if the player is grounded{
        if (dashCount <= 0)
        { // Check if the player has any dash count left{
            dashCount = maxDashCount; // Refill the dash count;   
        }
    }

    public void CheckForBulletTime()
    { // Check if the player can enter bullet time{
        if (canIntoBulletTime)
        { // If the player can enter bullet time{
            BulletTimeManager.instance.Enter(); // Enter bullet time
            BulletTimer = BulletTimeDuration;
            BulletTimeCooldownTimer = 0;
        }
        if (BulletTimeManager.instance.isBulletTime)
        { // If the game is in bullet time{
            BulletTimer -= Time.deltaTime;
            game_UI.ConsumeBulletTime(); //UI读条

            if (InputToExitBulletTime() || BulletTimer <= 0)
            {
                BulletTimeCooldownTimer = BulletTimer >= 0 ? BulletTimer : 0;
                BulletTimeManager.instance.Exit();
            }
        }
        else
        {
            BulletTimeCooldownTimer = Mathf.MoveTowards(BulletTimeCooldownTimer, BulletTimeDuration, BulletTimeRefillSpeed * Time.deltaTime);
            game_UI.ResetBulletTime();
        }
    }

    public bool InputToExitBulletTime()
    {
        if (!GameInput.IsBulletTimePressed())
            return true;
        if (GameInput.IsAttackPressed())
            return true;
        if (GameInput.IsDashPressed())
            return true;
        if (canRoll)
            return true;
        return false;
    }

    public void OnHit()
    {
        playerState.OnHit();
        game_UI.UpdateHealthBars();
    }

    public bool HeadWallCheck()
    { // Check if the player is touching a wall{
        List<Collider2D> results = new List<Collider2D>();
        int count = HeadWallCheckCollider.OverlapCollider(new ContactFilter2D { layerMask = wallLayer }, results);
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == LayerMask.NameToLayer("Ground"))
                return true;
        }
        return false;
    }

    public RaycastHit2D RightWallCheck()
    {
        int dir = facing == Facing.Right ? 1 : -1;
        Vector3 raycastPosition = boxCollider.bounds.center;
        raycastPosition.x += boxCollider.bounds.size.x / 2f * dir;
        return Physics2D.Raycast(raycastPosition, Vector2.right * dir, 0.3f, LayerMask.GetMask("Ground"));
    }

    public bool HaveWallAbove()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.up, 1f, LayerMask.GetMask("Ground"));
        if (hit.collider == null)
        {
            return false;
        }
        return true;
    }

    public void CounterBullet(Collider2D collider)
    {
        Bullet bullet = null;
        if ((bullet = collider.GetComponent<Bullet>()) != null && bullet.CompareTag("enemyAttack"))
        {
            bullet.Flip();
            effectController.CameraShake(Vector2.right);
        }
    }

    public IEnumerator Die()
    {
        DeadScreen.gameObject.SetActive(true);
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            DeadScreen.color = Color.Lerp(Color.clear, Color.black, t); // 逐渐显示黑色
            yield return null;
        }
        Game.instance.SaveGame();
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    public void PlayerDie()
    {
        fsm.ChangeState(fsm.GetState(State.Die));
    }

    public void SaveGameData(GameData gameData)
    {
        gameData.CheckPointPosition = CheckPointPosition; // 保存检查点位置
        gameData.PlayerHealth = playerState.currentHealth; // 保存玩家生命值
    }

    public void LoadGameData(GameData gameData)
    {
        CheckPointPosition = gameData.CheckPointPosition; // 加载检查点位置
        playerState = new PlayerState(this, maxHealth); // 创建新的玩家状态实例
        playerState.currentHealth = gameData.PlayerHealth; // 加载玩家生命值
        if (playerState.currentHealth <= 0)
        {
            playerState.currentHealth = playerState.maxHealth; // 如果生命值小于等于0，则重置为最大生命值
        }
        transform.position = CheckPointPosition + new Vector3(0, 2, 0);
        game_UI.UpdateHealthBars();
    }

    public void SetCheckPoint(Vector3 position)
    {
        CheckPointPosition = position;
    }
}
