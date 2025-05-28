using UnityEngine;

public class PlayerStandingAttackState : IState {
    private bool attackEnd;
    
    public AudioSource audioSource;
    public AudioClip standingAttackSound;
    public bool hasPlaySound = false;

    public PlayerStandingAttackState(Player player) : base(player)
    {
        stateName = "StandingAttack";
        state = State.StandingAttack;
    }

    public override State OnUpdate() {
        if(attackEnd) { 
            attackEnd = false;
            return State.Idle;
        }

        if (isAttackTriggered &&!hasPlaySound) { // Attack triggered function
            if (isAttackSuccess)
            {
                standingAttackSound = Game.instance.sceneManager.audioManager.GetAudioClip(SoundType.AttackSuccess); // Get the attack success sound            }  
            }
            audioSource.PlayOneShot(standingAttackSound); // Play the attack sound
            hasPlaySound = true; // Mark the attack sound as played
            Game.instance.sceneManager.audioManager.ReleaseAudioSource(audioSource); // Release the audio source
            audioSource = null; // Set the audio source to null
        }

        return state;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.attackTimer = player.attackCooldown; // Reset the attack timer
        attackEnd = false; // Reset the attack end flag

        standingAttackSound = Game.instance.sceneManager.audioManager.GetAudioClip(SoundType.Attacking2); // Get the standing attack sound
        audioSource = Game.instance.sceneManager.audioManager.GetAudioSource();
        hasPlaySound = false; // Reset the hasPlaySound flag
        isAttackTriggered = false; // Reset the isAttackTriggered flag
        isAttackSuccess = false; // Reset the isAttackSuccess flag
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