using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/HealAction")]
    public class HealAction : AIAction
    {
        public float healAmount = 2f;

        public override void Execute(Context context)
        {
            Debug.Log("HealAction activated");
            Debug.Log($"Health amount: { context.health.normalizedHealth}");
            if (context.healSpot == null) return;

            if (context.health != null)
            {
                context.health.Heal(healAmount);
                context.animator.SetBool("isRunning", false);
                Debug.Log($"Health amount: {context.health.GetHealth()}");
                Debug.Log("Boss healed");
            }
        }
    }
}