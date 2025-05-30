using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashState : IState
{
    private float dashTimer; // Timer for the dash duration
    private bool dashUp;
    private AudioSource dashAudioSource;
    private AudioClip dashAudioClip;
    public PlayerDashState(Player player) : base(player)
    {
        stateName = "Dash";
        state = State.Dash;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        player.dashCount--;
        dashAudioSource = player.soundEffectController.GetAudioSource();
        dashAudioClip = player.soundEffectController.GetSoundClip(SoundType.Dashing);

        PlayDashAudio();

        //在地面上播放冲刺特效
        if (player.IsGrounded())
        {
            GameObject fx = player.effectController.PlayerDashFX(player.GetComponentInChildren<SpriteRenderer>().transform.position);
            if (!(player.facing == Facing.Right))
            {
                fx.transform.Rotate(0, 180, 0);
            }
        }

        /// TODO:操作方式待定
        // Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world space
        // player.dashDirection = (mousePosition - (Vector2)player.transform.position).normalized; // Calculate the direction from the player to the mouse position

        player.dashDirection = GameInput.LastDir.normalized; // Get the last direction from the input manager

        if (player.dashDirection == Vector2.zero)
        { // If the direction is zero, set it to the player's facing direction{
            player.dashDirection = player.facing == Facing.Right ? Vector2.right : Vector2.left; // Set the dash direction to the player's facing direction
        }
        dashUp = false;
        if (player.dashDirection.y > 0 && player.dashDirection.x == 0)
        {
            dashUp = true;
        }

        player.SetAnimation("DashUp", dashUp);

        if (!dashUp)
            player.StartCoroutine(PlayDashTrail());

        /// 获取鼠标位置或者键盘决定位置

        // player.SetVelocity(player.dashDirection * player.dashSpeed); // Set the player's velocity to the dash direction and speed

        // if (dashUp)
        // {
        //     player.SetVelocity(player.dashDirection * player.upDashSpeed);
        // }

        float dashX = player.dashDirection.x * player.dashSpeed; // Calculate the x component of the dash velocity
        float dashY = player.dashDirection.y * player.upDashSpeed;
        player.SetVelocity(new Vector2(dashX, dashY)); // Set the player's velocity to the dash velocity

        dashTimer = player.dashDuration; // Reset the dash timer

        player.effectController.Freeze(0.05f);

        player.effectController.CameraShake(player.dashDirection); // Trigger camera shake effect
    }
    public override State OnUpdate()
    {
        dashTimer -= Time.deltaTime; // Decrement the dash timer
        if (dashTimer >= player.dashDuration * 0.2f)
        {    //冲刺前半段保持，不能被打断
            if (player.canAttack)
            {
                return State.DashAttack; // Return to attack state
            }
        }
        if (dashTimer <= 0) // If the dash timer has run out, change to the idle state
        {
            return State.Idle;
        }
        return state;
    }

    public override void OnExit()
    {
        base.OnExit();
        ReleaseDashAudio();
    }

    public IEnumerator PlayDashTrail()
    {
        for (float i = 0; i < player.dashDuration; i += player.trailFXInterval)
        {
            GameObject fx = player.effectController.PlayerDashTrailFX(player.animator.transform.position);
            if (!(player.facing == Facing.Right))
            {
                fx.transform.Rotate(0, 180, 0);
            }
            yield return new WaitForSeconds(player.trailFXInterval);
        }
    }

    private void PlayDashAudio()
    {
        if (dashAudioSource == null || dashAudioClip == null)
        {
            Debug.LogWarning("Dash audio source or clip is not set.");
            return;
        }
        dashAudioSource.PlayOneShot(dashAudioClip);
    }

    private void ReleaseDashAudio()
    {
        if (dashAudioSource == null || dashAudioClip == null)
        {
            Debug.LogWarning("Dash audio source or clip is not set.");
            return;
        }

        player.soundEffectController.ReleaseAudioSource(dashAudioSource);
        dashAudioSource = null;
    }
}