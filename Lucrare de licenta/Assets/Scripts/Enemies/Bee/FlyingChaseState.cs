using UnityEngine;

public class FlyingChaseState : FlyingEnemyState
{
    private Unit pathFollower;
    private float updatePathTimer = 0f;
    private float updatePathCooldown = 0.5f;
    public FlyingChaseState(FlyingEnemyFSM enemy) : base(enemy)
    {
        pathFollower = enemy.GetComponent<Unit>();
    }

    public override void EnterState()
    {
        Debug.Log("Intrat în starea de urmarire");
        enemy.anim.SetBool("moving", true);
        RequestPathToPlayer();
    }

    public override void UpdateState()
    {
        updatePathTimer += Time.deltaTime;

        if (Vector2.Distance(enemy.enemy.position, enemy.player.position) > enemy.stopChaseDistance)
        {
            enemy.ChangeState(new FlyingPatrolState(enemy));
            return;
        }

        FlipTowardsPlayer();

        if (updatePathTimer >= updatePathCooldown)
        {
            RequestPathToPlayer();
            updatePathTimer = 0f;
        }
    }

    private void RequestPathToPlayer()
    {
        PathRequestManager.RequestPath(enemy.enemy.position, enemy.player.position, pathFollower.OnPathFound);
    }

    private void FlipTowardsPlayer()
    {
        if ((enemy.player.position.x < enemy.enemy.position.x && enemy.enemy.localScale.x > 0) ||
            (enemy.player.position.x > enemy.enemy.position.x && enemy.enemy.localScale.x < 0))
        {
            enemy.enemy.localScale = new Vector3(-enemy.enemy.localScale.x, enemy.enemy.localScale.y, enemy.enemy.localScale.z);
        }
    }

    public override void ExitState()
    {
        enemy.anim.SetBool("moving", false);
    }
}
