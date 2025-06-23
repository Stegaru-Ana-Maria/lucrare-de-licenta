using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/SpearRecoverableConsideration")]
    public class SpearRecoverableConsideration : Consideration
    {
        public override float Evaluate(Context context)
        {
            if (context.spear == null)
                return 0f;

            return context.spear.IsRecoverable() ? 1f : 0f;
        }
    }
}
