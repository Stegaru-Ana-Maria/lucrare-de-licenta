using UnityEngine;
using UnityUtils;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/ThrowSpearDistanceConsideration")]
    public class ThrowSpearDistanceConsideration : Consideration
    {
        public float maxDistance = 20f;  
        public string targetTag = "Player";
        public AnimationCurve distanceCurve;

        public override float Evaluate(Context context)
        {
            if (!context.sensor.targetTags.Contains(targetTag))
            {
                context.sensor.targetTags.Add(targetTag);
            }

            Transform targetTransform = context.sensor.GetClosestTarget(targetTag);
            if (targetTransform == null) return 0f;

            Transform agentTransform = context.bossTransform;

            Vector3 directionToTarget = targetTransform.position - agentTransform.position;
            directionToTarget.y = 0; // ignor diferenta pe verticala

            float distanceToTarget = directionToTarget.magnitude;

            if (distanceToTarget > maxDistance)
                return 0f;

            float normalizedDistance = Mathf.Clamp01(distanceToTarget / maxDistance);

            float utility = distanceCurve.Evaluate(normalizedDistance);

            return Mathf.Clamp01(utility);
        }

        void Reset()
        {
            distanceCurve = new AnimationCurve(
                new Keyframe(0f, 0f),   // aproape - 0
                new Keyframe(0.3f, 0.2f), // la 30% din distanta - 0.2
                new Keyframe(0.7f, 1f),   // la 70% din distanta - 1
                new Keyframe(1f, 1f)     // la maxim - 1
            );
        }
    }
}
