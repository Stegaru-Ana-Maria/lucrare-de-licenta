using UnityEngine;

public class PatrolState : EnemyState
{
    private Vector3 targetPoint;
    private float idleTimer;
    private bool movingLeft;

    public PatrolState(EnemyFSM enemy) : base(enemy) { }

    public override void EnterState()
    {
        enemy.anim.SetBool("moving", true);
        targetPoint = enemy.patrolPointB.position;
    }

    public override void UpdateState()
    {
        enemy.enemy.position = Vector2.MoveTowards(enemy.enemy.position, targetPoint, enemy.patrolSpeed * Time.deltaTime);

        if ((targetPoint.x < enemy.enemy.position.x && enemy.enemy.localScale.x > 0) || (targetPoint.x > enemy.enemy.position.x && enemy.enemy.localScale.x < 0))
        {
            enemy.enemy.localScale = new Vector3(-enemy.enemy.localScale.x, enemy.enemy.localScale.y, enemy.enemy.localScale.z);
        }

            if (Vector2.Distance(enemy.enemy.position, enemy.player.position) < enemy.detectionRange)
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        if (Vector2.Distance(enemy.enemy.position, targetPoint) < 0.1f)
        {
            idleTimer += Time.deltaTime;
            enemy.anim.SetBool("moving", false);

            if (idleTimer > enemy.idleDuration)
            {
                movingLeft = !movingLeft;
                targetPoint = movingLeft ? enemy.patrolPointA.position : enemy.patrolPointB.position;
                enemy.anim.SetBool("moving", true);
                idleTimer = 0;
            }
        }
    }

    public override void ExitState()
    {
        enemy.anim.SetBool("moving", false);
    }
}
