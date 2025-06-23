using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/MeleeAttackAction")]
    public class MeleeAttackAIAction : AIAction
    {
        public string cooldownKey = "MeleeAttackCooldown";
        public float attackCooldown = 2f;

        public override void Execute(Context context)
        {
            float lastAttackTime = context.GetData<float>(cooldownKey);

            if (Time.time - lastAttackTime < attackCooldown)
                return;

            Debug.Log("Boss is performing a melee attack.");

            var player = context.sensor.GetClosestTarget("Player");
            
            if (player != null)
            {
                Vector3 bossPos = context.bossTransform.position;
                Vector3 playerPos = player.position;

                Vector3 scale = context.bossTransform.localScale;

                if (playerPos.x < bossPos.x)
                    scale.x = Mathf.Abs(scale.x) * -1f; 
                else
                    scale.x = Mathf.Abs(scale.x); 

                context.bossTransform.localScale = scale;
            }
            context.animator.SetTrigger("meleeAttack");
            context.animator.SetBool("isRunning", false);
        }
    }
}
