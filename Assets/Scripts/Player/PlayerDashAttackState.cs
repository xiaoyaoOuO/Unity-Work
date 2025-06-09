using UnityEngine;

public class PlayerDashAttackState : IState {
    private bool attackEnd;
    private bool AttackUp;

    public AudioSource audioSource;
    public AudioClip dashAttackSound;
    public bool hasPlaySound = false;
    public PlayerDashAttackState(Player player) : base(player)
    {
        stateName = "DashAttack";
        state = State.DashAttack;
    }

    public override State OnUpdate() {
        if(attackEnd) {
            attackEnd = false;
            return State.Idle;
        }

        if (isAttackTriggered && !hasPlaySound)
        {
            if (isAttackSuccess)
            {
                dashAttackSound = Game.instance.sceneManager.audioManager.GetAudioClip(SoundType.AttackSuccess);
            }
            audioSource.PlayOneShot(dashAttackSound);
            hasPlaySound = true;
            Game.instance.sceneManager.audioManager.ReleaseAudioSource(audioSource);
            audioSource = null;
        }

        return state;
    }

    public override void OnEnter() {
        base.OnEnter();

        hasPlaySound = false;
        isAttackTriggered = false;
        isAttackSuccess = false;

        dashAttackSound = Game.instance.sceneManager.audioManager.GetAudioClip(SoundType.Attacking2);
        audioSource = Game.instance.sceneManager.audioManager.GetAudioSource();

        player.attackTimer = player.attackCooldown; // Reset the attack timer
        this.AttackUp = player.dashDirection.y >  0 && player.dashDirection.x == 0;
        player.SetAnimation("DashAttackUp",this.AttackUp); 
        attackEnd = false; // Reset the attack end flag 
    }

    public override void OnExit() {
        base.OnExit(); 
    }

    public override void AnimationEndTrigger() {
        attackEnd = true; // Set the attackEnd flag to true when the animation ends 
    }

    public override void AnimationAttackTrigger(Collider2D collider = null) {
        base.AnimationAttackTrigger(player.RightAttackCollider);
    }

}