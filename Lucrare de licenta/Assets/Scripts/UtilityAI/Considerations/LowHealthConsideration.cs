using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/LowHealthConsideration")]
    public class LowHealthConsideration : Consideration
    {
        [Range(0f, 1f)]
        public float healthThreshold = 0.3f; 

        public override float Evaluate(Context context)
        {
            if (context.health == null) return 0f;

            float healthPercent = context.health.normalizedHealth;

            return healthPercent <= healthThreshold ? 1f : 0f;
        }
    }
}
