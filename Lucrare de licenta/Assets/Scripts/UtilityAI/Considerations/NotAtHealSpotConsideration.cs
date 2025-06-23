using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/NotAtHealSpotConsideration")]
    public class NotAtHealSpotConsideration : Consideration
    {
        public float proximityDistance = 3f;

        public override float Evaluate(Context context)
        {
            if (context.healSpot == null) return 0f;

            float distance = Vector2.Distance(context.bossTransform.position, context.healSpot.position);
            return distance > proximityDistance ? 1f : 0f;
        }
    }
}
