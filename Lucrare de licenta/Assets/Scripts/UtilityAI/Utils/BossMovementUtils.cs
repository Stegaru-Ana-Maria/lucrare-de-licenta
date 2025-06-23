using UnityEngine;

namespace UtilityAI
{
    public static class BossMovementUtils
    {
        public static void FlipTowardsTarget(Transform boss, Vector3 target)
        {
            float direction = target.x - boss.position.x;
            if (direction != 0)
            {
                boss.localScale = new Vector3(Mathf.Sign(direction) * Mathf.Abs(boss.localScale.x), boss.localScale.y, boss.localScale.z);
            }
        }

        public static bool IsObstacleAhead(Transform bossTransform, Context context)
        {
            Vector2 direction = bossTransform.localScale.x > 0 ? Vector2.right : Vector2.left;
            Vector2 origin = new Vector2(bossTransform.position.x, bossTransform.position.y + 0.1f);
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, context.obstacleCheckDistance, context.obstacleMask);

            Debug.DrawRay(origin, direction * context.obstacleCheckDistance, Color.green);

            return hit.collider != null;
        }

        public static bool IsGrounded(Transform bossTransform, Context context)
        {
            Vector2 origin = new Vector2(bossTransform.position.x, bossTransform.position.y - 0.5f);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, context.groundCheckDistance, context.obstacleMask);

            Debug.DrawRay(origin, Vector2.down * context.groundCheckDistance, Color.yellow);

            return hit.collider != null;
        }
    }
}
