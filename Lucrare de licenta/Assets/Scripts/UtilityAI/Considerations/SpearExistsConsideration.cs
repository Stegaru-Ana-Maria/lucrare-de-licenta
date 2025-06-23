using UnityEngine;
using UnityUtils;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/SpearExistsConsideration")]
    public class SpearExistsConsideration : Consideration
    {
        public override float Evaluate(Context context)
        {
            return context.spearTransform != null ? 1f : 0f;
        }
    }
}
