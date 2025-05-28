using System;
using UnityEngine;

public enum AttackDirection : int
{
    None = 0,
    Right,
    Up,
    Down, 
};
public class PlayerAttackState : IState {
    public bool attackEnd;
    int Dir;    //获取人物朝向

    //攻击音效
    public AudioClip attackSound;
    public AudioSource attackAudioSource;
    public bool hasPlaySound = false;
    public PlayerAttackState(Player player) : base(player)
    {
        stateName = "Attack";
        state = State.Attack;
    }

    public override State OnUpdate() {
        if(attackEnd) { 
            attackEnd = false;
            return State.Idle; // Return to idle state
        }

        if (isAttackTriggered && !hasPlaySound)
        { // 攻击触发时调用的函数
            if (isAttackSuccess)
            {
                attackSound = Game.instance.sceneManager.audioManager.GetAudioClip(SoundType.AttackSuccess); // 获取攻击成功音效
            }
            attackAudioSource.PlayOneShot(attackSound); // 播放攻击音效
            hasPlaySound = true; // 标记攻击音效已播放
            Game.instance.sceneManager.audioManager.ReleaseAudioSource(attackAudioSource); 
            attackAudioSource = null;
        }
        return state;
    }

    public override void OnEnter()
    {
        // player.dashCount--;   TODO : Dash count
        base.OnEnter();
        attackEnd = false;

        isAttackTriggered = false; 
        isAttackSuccess = false; 
        hasPlaySound = false; 

        attackSound = Game.instance.sceneManager.audioManager.GetAudioClip(SoundType.Attacking2);
        attackAudioSource = Game.instance.sceneManager.audioManager.GetAudioSource();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world space
        Vector2 attackDirection = (mousePosition - (Vector2)player.transform.position).normalized; // Calculate the direction from the player to the mouse position

        if (player.facing == Facing.Right)
        { // Right direction
            Dir = 1;
        }
        else
        { // Left direction
            Dir = -1;
        }

        float angle = Vector2.SignedAngle(Vector2.right * Dir, attackDirection); // Calculate the angle between the player and the mouse position

        Dir = GetAttackDirection(angle,Dir);

        player.SetAnimation("AttackDir", Dir);
    }

    private int GetAttackDirection(float angle, int direction)
    {
        if (angle > -90 && angle < 90)
        { // Right direction
            if (angle > 45)
            { // Up direction
                Dir = (int)AttackDirection.Up;
            }
            else if (angle < -45)
            { // Down direction
                Dir = (int)AttackDirection.Down;
            }
            else
            { // Right direction
                Dir = (int)AttackDirection.Right;
            }
        }
        else
        {
            player.OnFlip(); // Flip the player if the mouse is on the left side of the player
            if (angle < 135 && angle > 0)
            {
                Dir = (int)AttackDirection.Up; // Up direction
            }
            else if (angle > -135 && angle < 0)
            { // Down direction
                Dir = (int)AttackDirection.Down; // Down direction
            }
            else
            { // Right direction
                Dir = (int)AttackDirection.Right; // Right direction
            }
        }
        if(direction == -1){
            if(Dir == (int)AttackDirection.Up){
                Dir = (int)AttackDirection.Down; // Flip the direction if the player is facing left
            }
            else if(Dir == (int)AttackDirection.Down){
                Dir = (int)AttackDirection.Up; // Flip the direction if the player is facing left
            }
        }
        return Dir; // Return the attack direction  
    }

    public override void OnExit() {
        base.OnExit(); 
        player.attackTimer = player.attackCooldown; // Reset the attack timer
    }

    public override void AnimationEndTrigger() { // 攻击结束时调用的函数
        attackEnd = true; // 攻击结束
    }

    public override void AnimationAttackTrigger(Collider2D attackBox = null) { // 攻击时调用的函数
        isAttackTriggered = true; // 标记攻击已触发

        Collider2D[] hitEnemies = new Collider2D[10]; // Array to store hit enemies
        Collider2D collider = player.RightAttackCollider; 
        
        if (Dir == (int)AttackDirection.Right)
        { 
            collider = player.RightAttackCollider; 
        }
        else if (Dir == (int)AttackDirection.Up)
        { 
            collider = player.UpAttackCollider; 
        }
        else if (Dir == (int)AttackDirection.Down)
        { 
            collider = player.DownAttackCollider; 
        }

        collider.OverlapCollider(new ContactFilter2D { layerMask = player.enemyLayer }, hitEnemies); 
        for (int i = 0; i < hitEnemies.Length; i++) {
            if (hitEnemies[i]!= null) {
                Enemy enemy = hitEnemies[i].GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.OnHit();
                    Debug.Log("Hit enemy: " + enemy.name);  
                    isAttackSuccess = true;
                }
                else
                {
                    break;
                }
            } 
        }
    }

}