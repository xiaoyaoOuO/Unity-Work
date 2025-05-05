using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing{Left, Right}

public class Player : MonoBehaviour
{
    #region Player Variables
    public float speed = 5f; // Player speed
    public float jumpForce = 3f; // Player jump force
    public float gravity = 0.5f; // Player gravity
    public float attackCooldown = 1f; // Player attack range
    public float attackTimer;
    public float dashSpeed = 15f; // Player dash speed
    public float dashDuration = 0.3f; // Player dash duration
    public Vector2 dashDirection; // Player dash direction
    public int dashCount = 1;
    public int maxDashCount = 1;
    public Facing facing = Facing.Right;

    public bool canAttack{get{return attackTimer <= 0 && GameInput.IsAttackPressed();}}

    public bool canDash{get{return dashCount > 0 && GameInput.IsDashPressed();}}

    public bool canJump{get{return GameInput.IsJumpPressed() && IsGrounded();}}
    #endregion

    // Create a finite state machine for the player
    private FiniteStateMachine<IState> fsm;

    private Animator animator; // Reference to the Animator component

    public Rigidbody2D rb; // Reference to the Rigidbody2D component
    private BoxCollider2D boxCollider;


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
        animator = GetComponentInChildren<Animator>(); // Get the Animator component attached to the player
        rb = GetComponentInChildren<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
        boxCollider = GetComponentInChildren<BoxCollider2D>();

        rb.gravityScale = gravity; // Set the gravity scale of the Rigidbody2D component
    

        fsm.Initialize();        
    }

    // Update is called once per frame
    void Update()
    {
        // Update the state machine
        fsm.Update();

        UpdatePlayerController();
    }

    private void UpdatePlayerController()
    {
        if (IsGrounded() && rb.velocity.y <= 0)
        {
            RefillDash(); // Refill the dash count if the player is grounded{    
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
        if(facing == Facing.Left && velocity.x > 0 || facing == Facing.Right && velocity.x < 0) // Check if the player is moving in the opposite direction{
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
        if(hit.collider == null) { // If the player is not grounded, return false
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
        SetVelocity(new Vector2(rb.velocity.x, jumpForce)*1.5f);
    }

    public Vector2 getVelocity() => rb.velocity;
    public void AnimationTrigger() => fsm.CurrentState.AnimationEndTrigger();

    public void RefillDash() { // Refill the dash count if the player is grounded{
        if(dashCount <= 0 ) { // Check if the player has any dash count left{
            dashCount = maxDashCount; // Refill the dash count;   
        }
    }
}
