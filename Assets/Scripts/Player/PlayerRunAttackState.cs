using UnityEngine;

public class PlayerRunAttackState : IState {
    private bool attackEnd;
    private int slashAttackCount;

    public AudioSource audioSource;
    public AudioClip runAttackSound;
    public bool hasPlaySound = false;
    public PlayerRunAttackState(Player player) : base(player)
    {
        stateName = "RunAttack";
        state = State.RunAttack;
        slashAttackCount = 0;
    }

    public override State OnUpdate() {
        if (player.canDash)
        {
            return State.DashAttack;
        }
        if (player.canRoll)
        {
            return State.Roll;
        }
        if (attackEnd)
        {
            attackEnd = false;
            return State.Run; // Return to idle state
        }

        if (isAttackTriggered && !hasPlaySound)
        { // 攻击触发时调用的函数
            if (isAttackSuccess)
            {
                runAttackSound = Game.instance.sceneManager.audioManager.GetAudioClip(SoundType.AttackSuccess); // 获取攻击成功音效
            }
            audioSource.PlayOneShot(runAttackSound); // 播放攻击音效
            hasPlaySound = true; // 标记攻击音效已播放
            Game.instance.sceneManager.audioManager.ReleaseAudioSource(audioSource); // 释放音频源
            audioSource = null; // 将音频源设置为 null
        }

        float dir = Input.GetAxisRaw("Horizontal"); // Get the horizontal input direction
        player.SetVelocity(new Vector2(dir * player.speed, player.rb.velocity.y)); 
        return state;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        hasPlaySound = false; // Reset the hasPlaySound flag
        isAttackTriggered = false; // Reset the isAttackTriggered flag
        isAttackSuccess = false; // Reset the isAttackSuccess flag

        runAttackSound = Game.instance.sceneManager.audioManager.GetAudioClip(SoundType.Attacking2); // Get the run attack sound
        audioSource = Game.instance.sceneManager.audioManager.GetAudioSource();

        player.attackTimer = player.attackCooldown; // Reset the attack timer
        attackEnd = false; // Reset the attack end flag
        slashAttackCount = (slashAttackCount + 1) & 1;
        player.SetAnimation("RunSlash", slashAttackCount);
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