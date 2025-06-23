using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Considerations/PlayerInBossRoomConsideration")]
    public class PlayerInBossRoomConsideration : Consideration
    {
        public override float Evaluate(Context context)
        {
            return context.GetData<bool>("playerInBossRoom") ? 1f : 0f;
        }
    }
}