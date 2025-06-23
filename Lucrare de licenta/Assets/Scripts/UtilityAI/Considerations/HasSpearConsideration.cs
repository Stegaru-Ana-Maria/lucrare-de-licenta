using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/HasSpearConsideration")]
    public class HasSpearConsideration : Consideration
    {
        public override float Evaluate(Context context)
        {
            return context.hasSpear ? 1f : 0f;
        }
    }
}
