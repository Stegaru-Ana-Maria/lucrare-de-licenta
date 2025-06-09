using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UtilityAI
{
    public class Brain : MonoBehaviour
    {
        public List<AIAction> actions;
        public Context context;
        public Sensor sensor;
        public BossHealth health;
        public BossStats bossStats;
        public Transform playerTransform;
        public Transform spearTransform;
        public Transform healTreeTransform;

        void Awake()
        {
            context = new Context(this);
            sensor = GetComponent<Sensor>();
            health = GetComponent<BossHealth>();
            context.sensor = sensor;
            context.bossStats = bossStats;
            context.playerTransform = playerTransform;
            context.spearTransform = spearTransform;
            context.healTreeTransform = healTreeTransform;

            foreach (var action in actions)
            {
                action.Initialize(context);
            }
        }

        void Update()
        {
            UpdateContext();

            AIAction bestAction = null;
            float highestUtility = float.MinValue;

            foreach (var action in actions)
            {
                float utility = action.CalculateUtility(context);
                if (utility > highestUtility)
                {
                    highestUtility = utility;
                    bestAction = action;
                }
            }

            if (bestAction != null)
            {
                bestAction.Execute(context);
            }
        }

        void UpdateContext()
        {
            context.SetData("health", health.normalizedHealth);
           // context.SetData("health", bossStats.currentHP / bossStats.maxHP);
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            context.SetData("distanceToPlayer", distanceToPlayer);

            float distanceToSpear = Vector2.Distance(transform.position, spearTransform.position);
            context.SetData("distanceToSpear", distanceToSpear);

            float distanceToHealTree = Vector2.Distance(transform.position, healTreeTransform.position);
            context.SetData("distanceToHealTree", distanceToHealTree);
        }
    }
}
