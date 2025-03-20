using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyFSM enemy) : base(enemy) { }

    public override void EnterState()
    {
        enemy.anim.SetBool("moving", true);
    }

    public override void UpdateState()
    {
        float distanceToPlayer = Vector2.Distance(enemy.enemy.position, enemy.player.position);

        if (distanceToPlayer > enemy.stopChaseDistance)
        {
            enemy.ChangeState(new PatrolState(enemy));
            return;
        }

        if (distanceToPlayer < enemy.attackRange)
        {
            enemy.ChangeState(new AttackState(enemy));
            return;
        }

        int direction = (enemy.player.position.x > enemy.enemy.position.x) ? 1 : -1;
        enemy.enemy.localScale = new Vector3(Mathf.Abs(enemy.enemy.localScale.x) * direction, enemy.enemy.localScale.y, enemy.enemy.localScale.z);
        enemy.enemy.position += new Vector3(direction * enemy.chaseSpeed * Time.deltaTime, 0, 0);
    }

    public override void ExitState() { }
}
