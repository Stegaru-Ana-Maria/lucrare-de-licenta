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
        public Spear spear;
        public Transform spearTransform;
        public BossSpearThrow bossSpearThrow;
        public Transform healSpot;
        public Dictionary<string, object> data = new();
        public float attackRangeGizmo = 5f;
        public Color attackRangeColor = Color.red;
        public LayerMask obstacleMask;

        void Awake()
        {
            context = new Context(this);
            sensor = GetComponent<Sensor>();
            health = GetComponent<BossHealth>();
            context.sensor = sensor;
            context.bossStats = bossStats;
            context.playerTransform = playerTransform;
            context.spearTransform = spearTransform;
            context.spear = spear;
            context.healSpot = healSpot;
            bossStats = GetComponent<BossStats>();
            context.bossRigidbody = GetComponent<Rigidbody2D>();
            context.animator = GetComponent<Animator>();
            context.obstacleMask = obstacleMask;
            context.bossSpearThrow = GetComponent<BossSpearThrow>();
            context.health = health;

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
                Debug.Log($"Action: {action.name}, Utility: {utility}");
                if (utility > highestUtility)
                {
                    highestUtility = utility;
                    bestAction = action;
                }
            }
            /*
            if (bestAction != null)
            {
                bestAction.Execute(context);
            }
            */
            Debug.Log($"Best Action: {bestAction?.name ?? "NONE"}"); 
            bestAction?.Execute(context);
        }

        void UpdateContext()
        {
            context.SetData("health", health.normalizedHealth);

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            context.SetData("distanceToPlayer", distanceToPlayer);

            float distanceToSpear = Vector2.Distance(transform.position, spearTransform.position);
            context.SetData("distanceToSpear", distanceToSpear);

            float distanceToHealTree = Vector2.Distance(transform.position, healSpot.position);
            context.SetData("distanceToHealTree", distanceToHealTree);

        }

        public void DealMeleeDamage()
        {
            var player = context.sensor.GetClosestTarget("Player");
            if (player != null)
            {
                var damageable = player.GetComponent<Health>();
                if (damageable != null)
                {
                    SoundEffectManager.Play("MeleeAttack");
                    damageable.TakeDamage(1);
                }
            }
        }

        public void OnAttackAnimationEnd()
        {
            context.SetData("MeleeAttackCooldown", Time.time);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = attackRangeColor;
            Gizmos.DrawWireSphere(transform.position, attackRangeGizmo);
        }
    }
}
