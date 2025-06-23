using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/NotInMeleeRangeConsideration")]
    public class NotInMeleeRangeConsideration : Consideration
    {
        public float maxDistance = 5f;
        public float scoreWhenOutOfRange = 1f;

        public override float Evaluate(Context context)
        {
            float distanceToPlayer = context.GetData<float>("distanceToPlayer");

            bool isInRange = distanceToPlayer <= maxDistance;

            return isInRange ? 0f : Mathf.Clamp01(scoreWhenOutOfRange);
        }
    }
}