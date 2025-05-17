using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing{Left, Right}

public class Player : MonoBehaviour
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
    public bool canGrab { get { return GameInput.IsGrabPressed() && WallCheck(); } } // 是否可以抓墙
    public bool canAttack { get { return attackTimer <= 0 && GameInput.IsAttackPressed(); } }
    public bool canDash { get { return dashCount > 0 && GameInput.IsDashPressed(); } }
    public bool canJump { get { return GameInput.IsJumpPressed() && IsGrounded(); } }
    public bool canIntoBulletTime { get { return GameInput.IsBulletTimePressed() && BulletTimeCooldownTimer >= BulletTimeDuration; } }
    public bool canRoll { get { return GameInput.IsRollPressed() && IsGrounded(); } }
    public Collider2D UpAttackCollider; // Reference to the UpAttack collider
    public Collider2D DownAttackCollider; // Reference to the DownAttack collider
    public Collider2D RightAttackCollider; // Reference to the RightAttack collider
    public LayerMask enemyLayer; // Layer mask for enemies

    private PlayerState playerState;
    #endregion

    // Create a finite state machine for the player
    private FiniteStateMachine<IState> fsm;

    private Animator animator; // Reference to the Animator component

    public Rigidbody2D rb; // Reference to the Rigidbody2D component
    private BoxCollider2D boxCollider;

    public IEffectController effectController;
    public ICamera cameraManager; // Reference to the camera manager

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

        //玩家状态
        playerState = new PlayerState(this);

        //获取组件
        animator = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();

        rb.gravityScale = gravity;

        //特效组
        effectController = Game.instance.sceneManager;
        cameraManager = Game.instance.cameraManager;

        //跳跃检查器
        jumpCheck = new JumpCheck(this);

        fsm.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
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
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
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
    public void AnimationTrigger() => fsm.CurrentState.AnimationEndTrigger();
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
            BulletTimer -= Time.deltaTime; // Decrement the bullet time cooldown
            if (InputToExitBulletTime() || BulletTimer <= 0)
            { // If the player is not pressing the bullet time button or the bullet time cooldown is less than or equal to 0{
                BulletTimeManager.instance.Exit(); // Exit bullet time
            }
        }
        else
        {
            BulletTimeCooldownTimer = Mathf.MoveTowards(BulletTimeCooldownTimer, BulletTimeDuration, BulletTimeRefillSpeed * Time.deltaTime);
        }
    }

    public bool InputToExitBulletTime()
    { // Check if the player can exit bullet time{
        if (!GameInput.IsBulletTimePressed())
            return true;
        if (GameInput.IsAttackPressed())
            return true;
        if (GameInput.IsDashPressed())
            return true;
        return false;
    }

    public void OnHit()
    {
        playerState.OnHit();
    }

    public bool WallCheck()
    { // Check if the player is touching a wall{
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.right * (facing == Facing.Right ? 1 : -1), 0.3f, LayerMask.GetMask("Ground"));
        if (hit.collider == null)
        { // If the player is not touching a wall, return false{
            return false;
        }
        return true;
    }
}
