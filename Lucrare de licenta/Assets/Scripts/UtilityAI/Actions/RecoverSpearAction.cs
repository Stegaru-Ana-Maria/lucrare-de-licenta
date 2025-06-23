using UnityEngine;
using UtilityAI;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/RecoverSpearAction")]
    public class RecoverSpearAction : AIAction
    {
        private Spear spearToRecover;

        public override void Execute(Context context)
        {
            Rigidbody2D rb = context.bossRigidbody;
            Animator animator = context.animator;

            Debug.Log("Executing RecoverSpearAction");

            if (spearToRecover == null || !spearToRecover.gameObject.activeInHierarchy)
            {
                Spear[] spearsInScene = GameObject.FindObjectsByType<Spear>(FindObjectsSortMode.None);

                if (spearsInScene.Length == 0)
                {
                    Debug.Log("No spears found to recover.");
                    return;
                }

                float closestDistance = Mathf.Infinity;
                Spear closestSpear = null;

                foreach (var spear in spearsInScene)
                {
                    if (spear.HasHitSomething()) 
                    {
                        float distance = Vector2.Distance(context.bossTransform.position, spear.transform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestSpear = spear;
                        }
                    }
                }

                if (closestSpear == null)
                {
                    Debug.Log("No valid spear found to recover.");
                    return;
                }

                spearToRecover = closestSpear;
            }

            BossMovementUtils.FlipTowardsTarget(context.bossTransform, spearToRecover.transform.position);

            float moveSpeed = context.speed; 

            if(context.bossSpearThrow.HasSpear == true)
            {
                Debug.Log("Boss collected the spear. -recover");
                Destroy(spearToRecover.gameObject);
                context.hasSpear = true;
                animator.SetBool("isRunning", false);
                spearToRecover = null;
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
                context.bossTransform.position,
                spearToRecover.transform.position,
                moveSpeed * Time.deltaTime
            );

            context.bossTransform.position = newPos;
            animator.SetBool("isRunning", true);
        }
    }
}
