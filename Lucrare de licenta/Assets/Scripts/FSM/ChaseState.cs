using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyFSM enemy) : base(enemy) { }

    public override void EnterState()
    {
        Debug.Log("Enter: ChaseState");
        enemy.anim.SetBool("moving", true);
    }

    public override void UpdateState()
    {
        float distanceToPlayer = Vector2.Distance(enemy.enemy.position, enemy.player.position);
        Vector2 enemyPos = enemy.enemy.position;
        Vector2 playerPos = enemy.player.position;

        float horizontalDistance = Mathf.Abs(playerPos.x - enemyPos.x);
        float verticalDistance = Mathf.Abs(playerPos.y - enemyPos.y);

        Vector2 directionToPlayer = (playerPos - enemyPos).normalized;
        Vector2 raycastOrigin = new Vector2(enemyPos.x, enemy.enemyCollider.bounds.min.y + 0.1f);
        float groundCheckDistance = 3f;
        int horizontalDirection = (playerPos.x > enemyPos.x) ? 1 : -1;
        RaycastHit2D groundHit = Physics2D.Raycast(raycastOrigin, new Vector2(horizontalDirection, 0), groundCheckDistance, enemy.obstacleLayer);

        RaycastHit2D hit = Physics2D.Linecast(enemy.enemy.position, enemy.player.position, enemy.obstacleLayer);

        if (groundHit.collider != null && groundHit.collider.gameObject != enemy.player.gameObject)
        {
            Debug.Log("obstacol");
            enemy.ChangeState(new PatrolState(enemy));
            return;
        }
        if (distanceToPlayer < enemy.attackRange)
        {
            enemy.ChangeState(new AttackState(enemy));
            return;
        }
        if (distanceToPlayer > enemy.stopChaseDistance)
        {
            enemy.ChangeState(new PatrolState(enemy));
            return;
        }

        if (hit.collider != null)
        {
            enemy.ChangeState(new PatrolState(enemy));
            return;
        }

        if (horizontalDistance <= enemy.horizontalChaseTolerance)
        {
            enemy.anim.SetBool("moving", false);
            return;
        }

        if (verticalDistance > enemy.maxVerticalChaseDistance)
        {
            Debug.Log("Player prea sus, renunt la chase");
            enemy.ChangeState(new PatrolState(enemy));
            return;
        }

        int direction = (enemy.player.position.x > enemy.enemy.position.x) ? 1 : -1;
        enemy.enemy.localScale = new Vector3(Mathf.Abs(enemy.enemy.localScale.x) * direction, enemy.enemy.localScale.y, enemy.enemy.localScale.z);
        enemy.enemy.position += new Vector3(direction * enemy.chaseSpeed * Time.deltaTime, 0f, 0f);

        Debug.DrawLine(enemyPos, playerPos, Color.red);
        Debug.DrawRay(raycastOrigin, new Vector2(horizontalDirection * groundCheckDistance, 0), Color.blue);
    }

    private void DrawBoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, Color color)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector2 halfSize = size * 0.5f;

        Vector2 topLeft = origin + (Vector2)(rotation * new Vector3(-halfSize.x, halfSize.y, 0));
        Vector2 topRight = origin + (Vector2)(rotation * new Vector3(halfSize.x, halfSize.y, 0));
        Vector2 bottomLeft = origin + (Vector2)(rotation * new Vector3(-halfSize.x, -halfSize.y, 0));
        Vector2 bottomRight = origin + (Vector2)(rotation * new Vector3(halfSize.x, -halfSize.y, 0));

        // box-ul de start
        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
        Debug.DrawLine(bottomLeft, topLeft, color);

        // box-ul de la capatul distantei
        Vector2 move = (Vector2)(direction.normalized * distance);
        Debug.DrawLine(topLeft + move, topRight + move, color);
        Debug.DrawLine(topRight + move, bottomRight + move, color);
        Debug.DrawLine(bottomRight + move, bottomLeft + move, color);
        Debug.DrawLine(bottomLeft + move, topLeft + move, color);

        // margini
        Debug.DrawLine(topLeft, topLeft + move, color);
        Debug.DrawLine(topRight, topRight + move, color);
        Debug.DrawLine(bottomLeft, bottomLeft + move, color);
        Debug.DrawLine(bottomRight, bottomRight + move, color);
    }

    public override void ExitState() 
    {
        Debug.Log("Exit: ChaseState");
    }
}
