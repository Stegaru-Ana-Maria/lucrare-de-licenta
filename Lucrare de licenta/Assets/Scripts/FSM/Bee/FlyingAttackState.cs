using UnityEngine;

public class FlyingAttackState : FlyingEnemyState
{
    private float attackCooldown;
    private float lastAttackTime;
    private FlyingEnemyAttack attackComponent;

    public FlyingAttackState(FlyingEnemyFSM enemy) : base(enemy)
    {
        attackCooldown = enemy.attackCooldown;
        lastAttackTime = Time.time;
        attackComponent = enemy.GetComponent<FlyingEnemyAttack>();
    }

    public override void EnterState()
    {
        Debug.Log("Enter: AttackState");
        enemy.anim.SetTrigger("attack");
        lastAttackTime = Time.time;

        Collider2D playerCollider = Physics2D.OverlapCircle(enemy.enemy.position, 1f, enemy.playerLayer);
        if (playerCollider != null)
        {
            attackComponent.AttackPlayer(playerCollider);
        }
    }

    public override void UpdateState()
    {
        FacePlayer();

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            attackComponent.ResetAttack();
            enemy.ChangeState(new FlyingChaseState(enemy)); 
        }
    }

    private void FacePlayer()
    {
        if (enemy.player.position.x > enemy.enemy.position.x)
            enemy.enemy.localScale = new Vector3(Mathf.Abs(enemy.enemy.localScale.x), enemy.enemy.localScale.y, enemy.enemy.localScale.z);
        else
            enemy.enemy.localScale = new Vector3(-Mathf.Abs(enemy.enemy.localScale.x), enemy.enemy.localScale.y, enemy.enemy.localScale.z);
    }

    public override void ExitState() 
    {
        Debug.Log("Exit: AttackState");
    }
}

