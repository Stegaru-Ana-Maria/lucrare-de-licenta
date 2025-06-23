using UnityEngine;
using UnityUtils;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/NotFullHealthConsideration")]
    public class NotFullHealthConsideration : Consideration
    {
        public float scoreWhenNotFull = 1f;
        public float scoreWhenFull = 0f;

        public override float Evaluate(Context context)
        {
            if (context.health == null)
                return 0f;

            return context.health.normalizedHealth < 1f ? scoreWhenNotFull : scoreWhenFull;
        }
    }
}
