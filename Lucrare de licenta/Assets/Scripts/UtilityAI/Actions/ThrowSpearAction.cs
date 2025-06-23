using UnityEngine;
using UtilityAI;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/ThrowSpearAction")]
    public class ThrowSpearAction : AIAction
    {
        private BossSpearThrow spearThrower;
        private Transform target;

        public override void Execute(Context context)
        {
            Debug.Log("Activating Throw Spear Action");

            var spearThrower = context.bossTransform.GetComponent<BossSpearThrow>();
            context.animator.SetTrigger("throwSpear");
            context.animator.SetBool("isRunning", false);

            if (spearThrower == null)
            {
                Debug.LogError("No BossSpearThrow component found on boss!");
                return;
            }

            target = context.sensor.GetClosestTarget("Player");

            if (target == null)
            {
                Debug.LogWarning("No target found to throw spear at.");
                return;
            }

            if (target != null)
            {
                Vector3 bossPos = context.bossTransform.position;
                Vector3 playerPos = target.position;

                Vector3 scale = context.bossTransform.localScale;

                if (playerPos.x < bossPos.x)
                    scale.x = Mathf.Abs(scale.x) * -1f;
                else
                    scale.x = Mathf.Abs(scale.x);

                context.bossTransform.localScale = scale;
            }

            spearThrower.SetTarget(target);

            bool didShoot = spearThrower.TryShoot();

            if (didShoot)
            {
                Debug.Log("Spear thrown successfully.");
                context.hasSpear = false;
            }
            else
            {
                Debug.Log("Failed to throw spear.");
            }
        }
    }
}
