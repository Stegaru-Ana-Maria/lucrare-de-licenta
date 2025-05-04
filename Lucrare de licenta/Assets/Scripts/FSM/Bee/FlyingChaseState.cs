using UnityEngine;

public class FlyingChaseState : FlyingEnemyState
{
    private Unit pathFollower;
    private float pathUpdateTimer = 0f;
    private const float pathUpdateCooldown = 0.5f;
    private Vector2 lastPlayerPosition;

    public FlyingChaseState(FlyingEnemyFSM enemy) : base(enemy)
    {
        pathFollower = enemy.GetComponent<Unit>();
    }

    public override void EnterState()
    {
        Debug.Log("Enter: ChaseState");
        lastPlayerPosition = enemy.player.position;
        RequestPathToPlayer();
    }

    public override void UpdateState()
    {
        pathUpdateTimer += Time.deltaTime;

        if (Vector2.Distance(enemy.enemy.position, enemy.player.position) > enemy.stopChaseDistance)
        {
            enemy.ChangeState(new ReturnToPatrolState(enemy));
            return;
        }

        FlipTowardsPlayer();

        if (pathUpdateTimer >= pathUpdateCooldown && HasPlayerMovedSignificantly())
        {
            RequestPathToPlayer();
            pathUpdateTimer = 0f;
            lastPlayerPosition = enemy.player.position;
        }
    }

    private bool HasPlayerMovedSignificantly()
    {
        return Vector2.Distance(enemy.player.position, lastPlayerPosition) > 0.5f;
    }

    private void RequestPathToPlayer()
    {
        PathRequestManager.RequestPath(enemy.enemy.position, enemy.player.position, pathFollower.OnPathFound);
    }

    private void FlipTowardsPlayer()
    {
        float direction = enemy.player.position.x - enemy.enemy.position.x;
        if ((direction < 0 && enemy.enemy.localScale.x > 0) || (direction > 0 && enemy.enemy.localScale.x < 0))
        {
            enemy.enemy.localScale = new Vector3(-enemy.enemy.localScale.x, enemy.enemy.localScale.y, enemy.enemy.localScale.z);
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exit: ChaseState");
        pathFollower.StopPathFollowing();
    }
}
