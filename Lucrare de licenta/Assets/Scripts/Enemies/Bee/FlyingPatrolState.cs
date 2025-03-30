using UnityEngine;

public class FlyingPatrolState : FlyingEnemyState
{
    private Vector3 targetPoint;
    private float idleTimer;
    private bool movingLeft;

    public FlyingPatrolState(FlyingEnemyFSM enemy) : base(enemy) { }

    public override void EnterState()
    {
        Debug.Log("Intrat în starea de patrulare");
        targetPoint = enemy.patrolPointB.position;
    }

    public override void UpdateState()
    {
        enemy.enemy.position = Vector2.MoveTowards(enemy.enemy.position, targetPoint, enemy.patrolSpeed * Time.deltaTime);

        FlipTowardsTarget();

        if (Vector2.Distance(enemy.enemy.position, enemy.player.position) < enemy.detectionRange)
        {
            enemy.ChangeState(new FlyingChaseState(enemy));
            return;
        }

        if (Vector2.Distance(enemy.enemy.position, targetPoint) < 0.1f)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer > enemy.idleDuration)
            {
                movingLeft = !movingLeft;
                targetPoint = movingLeft ? enemy.patrolPointA.position : enemy.patrolPointB.position;
                idleTimer = 0;
            }
        }
    }

    private void FlipTowardsTarget()
    {
        float direction = targetPoint.x - enemy.enemy.position.x;
        if ((direction < 0 && enemy.enemy.localScale.x > 0) || (direction > 0 && enemy.enemy.localScale.x < 0))
        {
            enemy.enemy.localScale = new Vector3(-enemy.enemy.localScale.x, enemy.enemy.localScale.y, enemy.enemy.localScale.z);
        }
    }

    public override void ExitState() 
    {
        Debug.Log("Inamicul a iesit din starea de patrulare");
    }

}