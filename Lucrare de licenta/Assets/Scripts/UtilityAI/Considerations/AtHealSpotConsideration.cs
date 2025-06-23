using UnityEngine;
using UnityUtils;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/AtHealSpotConsideration")]
    public class AtHealSpotConsideration : Consideration
    {
        public float healRange = 3f; 
        public float scoreWhenAtSpot = 1f;
        public float scoreWhenNotAtSpot = 0f;

        public override float Evaluate(Context context)
        {
            if (context.healSpot == null)
                return 0f;

            float distance = Vector2.Distance(context.bossTransform.position, context.healSpot.position);
            return distance <= healRange ? scoreWhenAtSpot : scoreWhenNotAtSpot;
        }
    }
}