using UnityEngine;

namespace UtilityAI
{
    [CreateAssetMenu(menuName = "UtilityAI/Actions/MoveToTargetAction")]
    public class MoveToTargetAction : AIAction
    {
        public float moveSpeed = 5f;
        public override void Initialize(Context context)
        {
            context.sensor.targetTags.Add(targetTag);
        }

        public override void Execute(Context context)
        {
            var target = context.sensor.GetClosestTarget(targetTag);
            if (target == null) return;

            context.target = target;

            Vector2 direction = (target.position - context.brain.transform.position).normalized;
            Vector2 newPosition = Vector2.MoveTowards(
                context.brain.transform.position,
                new Vector2(target.position.x, context.brain.transform.position.y), 
                moveSpeed * Time.deltaTime
            );

            context.brain.transform.position = newPosition;
        }
    }
}