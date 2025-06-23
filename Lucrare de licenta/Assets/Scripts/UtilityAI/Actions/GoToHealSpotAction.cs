using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/GoToHealSpotAction")]
    public class GoToHealSpotAction : AIAction
    {
        public override void Execute(Context context)
        {
            if (context.healSpot == null)
            {
                Debug.LogWarning("No heal spot assigned in context.");
                return;
            }

            Rigidbody2D rb = context.bossRigidbody;
            Animator animator = context.animator;

            float moveSpeed = context.speed;

            if (context.healSpot.position.x < context.bossTransform.position.x && context.bossTransform.localScale.x > 0 ||
                context.healSpot.position.x > context.bossTransform.position.x && context.bossTransform.localScale.x < 0)
            {
                context.bossTransform.localScale = new Vector3(-context.bossTransform.localScale.x, context.bossTransform.localScale.y, context.bossTransform.localScale.z);
            }

            if (BossMovementUtils.IsObstacleAhead(context.bossTransform, context) && BossMovementUtils.IsGrounded(context.bossTransform, context))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, context.jumpForce);
                animator.SetTrigger("jump");
                SoundEffectManager.Play("EnemyJump");
            }

            Vector2 newPos = Vector2.MoveTowards(
                context.bossTransform.position,
                context.healSpot.position,
                moveSpeed * Time.deltaTime
            );

            context.bossTransform.position = newPos;

            animator.SetBool("isRunning", true);
        }
    }
}
