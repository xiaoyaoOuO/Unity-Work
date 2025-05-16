using UnityEngine;

public class PlayerStandingAttackState : IState {
    private bool attackEnd;
    public PlayerStandingAttackState(Player player) : base(player) {
        stateName = "StandingAttack";
        state = State.StandingAttack;
    }

    public override State OnUpdate() {
        if(attackEnd) { 
            attackEnd = false;
            return State.Idle;
        }
        return state;
    }

    public override void OnEnter() {
        base.OnEnter();
        player.attackTimer = player.attackCooldown; // Reset the attack timer
        attackEnd = false; // Reset the attack end flag
    }

    public override void OnExit() {
        base.OnExit();
    }

    public override void AnimationEndTrigger() {
       attackEnd = true; // Set the attackEnd flag to true when the animation ends
    }

    public override void AnimationAttackTrigger() {
        Collider2D[] hitEnemies = new Collider2D[10]; // Array to store hit enemies
        player.RightAttackCollider.OverlapCollider(new ContactFilter2D { layerMask = player.enemyLayer }, hitEnemies); // Check for enemies in the attack range 
        for (int i = 0; i < hitEnemies.Length; i++) {
            if (hitEnemies[i]!= null) {
                Mushroom enemy = hitEnemies[i].GetComponent<Mushroom>(); // Get the Mushroom component from the hit enemy
                if (enemy!= null) {
                    enemy.OnHit(); // Call the OnHit method on the enemy
                    Debug.Log("Hit enemy: " + enemy.name); // Log the hit enemy  
                }else{
                    break;
                }
            } 
        }
    }
}