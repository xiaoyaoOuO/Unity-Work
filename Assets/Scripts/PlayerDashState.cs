using UnityEngine;

public class PlayerDashState : IState
{
    private float dashTimer; // Timer for the dash duration
    public PlayerDashState(Player player) : base(player) {
        stateName = "Dash";
        state = State.Dash; 
    }
    public override void OnEnter()
    {
        base.OnEnter();
        player.dashCount--; 

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world space
        player.dashDirection = (mousePosition - (Vector2)player.transform.position).normalized; // Calculate the direction from the player to the mouse position

        player.rb.gravityScale = 0;

        player.SetVelocity(player.dashDirection * player.dashSpeed); // Set the player's velocity to the dash direction and speed

        dashTimer = player.dashDuration; // Reset the dash timer
    }
    public override State OnUpdate()
    {
        dashTimer -= Time.deltaTime; // Decrement the dash timer
        if(dashTimer >= player.dashDuration * 0.5f){    //冲刺前半段保持，不能被打断
            return state;
        }
        if(player.canAttack) { // Check if the player is pressing the attack button
            return State.Attack; // Return to attack state
        }
        if (dashTimer <= 0) // If the dash timer has run out, change to the idle state
        {
            return State.Idle;
        }
        return state;
    }

    public override void OnExit(){
        base.OnExit();
        player.rb.velocity = Vector2.zero;
        player.rb.gravityScale = player.gravity;
    }
}