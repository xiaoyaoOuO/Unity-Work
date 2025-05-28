
using UnityEngine;
using System.Collections;

public class PlayerRollAttackState : IState
{
    private bool rollAttackEnd;
    private float acceleration;

    public AudioSource audioSource;
    public AudioClip rollAttackSound;
    public bool hasPlaySound = false;
    public PlayerRollAttackState(Player player) : base(player)
    {
        state = State.RollAttack;
        stateName = "RollAttack";
        acceleration = 20f;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        rollAttackEnd = false;
        hasPlaySound = false;
        isAttackTriggered = false;
        isAttackSuccess = false;

        rollAttackSound = Game.instance.sceneManager.audioManager.GetAudioClip(SoundType.Attacking2);
        audioSource = Game.instance.sceneManager.audioManager.GetAudioSource();
    }

    public override State OnUpdate()
    {
        if (rollAttackEnd)
        {
            return State.Idle;
        }
        Vector2 velocity = player.rb.velocity;
        velocity.x = Mathf.MoveTowards(velocity.x, 0, acceleration * Time.deltaTime);
        player.rb.velocity = velocity;

        if (isAttackTriggered && !hasPlaySound)
        {
            if (isAttackSuccess)
            {
                rollAttackSound = Game.instance.sceneManager.audioManager.GetAudioClip(SoundType.AttackSuccess);
            }
            audioSource.PlayOneShot(rollAttackSound);
            hasPlaySound = true;
            Game.instance.sceneManager.audioManager.ReleaseAudioSource(audioSource); 
            audioSource = null;
        }
        // Debug.Log(player.rb.velocity);
            return state;
    }

    public override void AnimationEndTrigger()
    {
        rollAttackEnd = true;
    }

    public override void AnimationAttackTrigger(Collider2D collider = null)
    {
        base.AnimationAttackTrigger(player.RightAttackCollider);
    }
}
