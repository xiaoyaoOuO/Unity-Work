using System;
using UnityEngine;

public enum State
{
    Idle,
    Run,
    Air,
    Attack,
    Hit,
    Die,
    Dash,
    Jump,
    RunAttack,
    StandingAttack,
    DashAttack,
    Roll,
    RollAttack,
    WallClimb,
    WallJump,
    WallIdle,
    Wall,
    CrossWall
}
public abstract class IState
{
    public State state { get; protected set; }
    public Player player;
    public String stateName { get; protected set; }
    public IState(Player player) { this.player = player; }
    public bool triggerCalled { get; protected set; }

    public virtual void OnEnter()
    {
        player.SetAnimation(stateName);
    }
    public virtual void OnExit()
    {
        player.SetAnimation(stateName, false);
    }
    public abstract State OnUpdate();

    public virtual void AnimationEndTrigger() { triggerCalled = true; }
    public virtual void AnimationAttackTrigger(Collider2D attackBox = null)
    {
        if (attackBox == null)
        {
            Debug.Log("Attack box is null");
            return;
        }
        Collider2D[] hitEnemies = new Collider2D[10]; // Array to store hit enemies
        attackBox.OverlapCollider(new ContactFilter2D { layerMask = player.enemyLayer }, hitEnemies); // Check for enemies in the attack range
        for (int i = 0; i < hitEnemies.Length; i++) {
            if (hitEnemies[i] != null) {
                Enemy enemy = hitEnemies[i].GetComponent<Enemy>();
                if (enemy != null) {
                    enemy.OnHit(); // Call the OnHit method on the enemy
                    Debug.Log("Hit enemy: " + enemy.name); // Log the hit enemy
                }
            }
        }
    }
    public virtual void FirstAnimationTrigger() { }
}