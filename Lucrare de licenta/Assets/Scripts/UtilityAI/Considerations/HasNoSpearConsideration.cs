using UnityEngine;
using UnityUtils;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/HasNoSpearConsideration")]
    public class HasNoSpearConsideration : Consideration
    {
        public override float Evaluate(Context context)
        {
            return context.hasSpear ? 0f : 1f;
        }
    }
}
