using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/ChasePlayerAction")]
    public class ChasePlayerAction : AIAction
    {
        public override void Execute(Context context)
        {
            Rigidbody2D rb = context.bossRigidbody;
            Animator animator = context.animator;

            Debug.Log("Executing ChasePlayerAction");

            if (context.playerTransform == null)
            {
                Debug.LogWarning("No player assigned in context.");
                return;
            }

            Vector3 playerPos = context.playerTransform.position;
            Vector3 bossPos = context.bossTransform.position;
            float moveSpeed = context.speed;

            BossMovementUtils.FlipTowardsTarget(context.bossTransform, playerPos);

            float distanceToPlayer = Vector2.Distance(bossPos, playerPos);
            if (distanceToPlayer <= context.stopDistance)
            {
                animator.SetBool("isRunning", false);
                return;
            }

            if (BossMovementUtils.IsObstacleAhead(context.bossTransform, context) && BossMovementUtils.IsGrounded(context.bossTransform, context))
            {
                Debug.Log("Obstacle ahead while chasing, boss jumps.");
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, context.jumpForce);
                animator.SetTrigger("jump");
                SoundEffectManager.Play("EnemyJump");
            }

            Vector2 newPos = Vector2.MoveTowards(
                bossPos,
                new Vector2(playerPos.x, bossPos.y), 
                moveSpeed * Time.deltaTime
            );

            context.bossTransform.position = newPos;
            animator.SetBool("isRunning", true);
        }
    }
}
