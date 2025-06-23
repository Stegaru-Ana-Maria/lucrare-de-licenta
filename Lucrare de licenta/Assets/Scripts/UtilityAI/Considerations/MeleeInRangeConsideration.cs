using UnityEngine;
using UnityUtils;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/MeleeInRangeConsideration")]
    public class MeleeInRangeConsideration : Consideration
    {
        public float maxDistance = 5f;
        public float maxAngle = 180f;
        public string targetTag = "Player";
        public float scoreWhenInRange = 0.8f;

        public override float Evaluate(Context context)
        {
            if (!context.sensor.targetTags.Contains(targetTag))
            {
                context.sensor.targetTags.Add(targetTag);
            }

            Transform targetTransform = context.sensor.GetClosestTarget(targetTag);
            if (targetTransform == null) return 0f;

            Transform agentTransform = context.bossTransform;

            bool isInRange = agentTransform.InRangeOf(targetTransform, maxDistance, maxAngle);

            return isInRange ? Mathf.Clamp01(scoreWhenInRange) : 0f;
        }
    }
}
