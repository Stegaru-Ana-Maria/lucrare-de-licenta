using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/CooldownConsideration")]
    public class CooldownConsideration : Consideration
    {
        public string cooldownKey = "MeleeAttackCooldown";
        public float cooldownTime;

        public override float Evaluate(Context context)
        {
            float lastTime = context.GetData<float>(cooldownKey);
            float timeSinceLastUse = Time.time - lastTime;

            float score = timeSinceLastUse >= cooldownTime ? 1f : 0f;

            Debug.Log($"CooldownConsideration {cooldownKey} — time since last: {timeSinceLastUse}, score: {score}");

            return score;
        }
    }
}
