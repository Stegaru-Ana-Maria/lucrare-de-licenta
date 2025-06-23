using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/IdleAction")]
    public class IdleAIAction : AIAction
    {
        public override void Execute(Context context)
        {
            Debug.Log("Boss is idling.");
            context.animator.SetBool("isRunning", false);
            context.animator.Play("Idle");
        }
    }
}