using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
//using UnityEditor.Experimental.GraphView;

public class Boss2AI : MonoBehaviour
{
    private Stopwatch btStopwatch = new Stopwatch();
    [SerializeField] public float btEvaluationTime;

    [Header("Stats")]
    [SerializeField] private float maxHP = 10f;
    public float currentHP;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private SpriteRenderer bossSprite;
    [SerializeField] private Color rageColor = Color.red;

    [Header("Ranges")]
    [SerializeField] private float attack1Range = 3.5f;
    [SerializeField] private float attack2Range = 3f;
    [SerializeField] private float specialAttackRange = 7f;

    [Header("Cooldowns")]
    [SerializeField] private float attackCooldownTime = 2f;
    [SerializeField] private float healCooldown = 10f;
    [SerializeField] private float rageCooldownTime = 1f;

    [Header("Dependencies")]
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attack1Duration = 0.5f;
    [SerializeField] private float attack2Duration = 1.6f;

    [Header("Special Attack - Crystal")]
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private Transform crystalSpawnPoint;
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private GameObject healEffectPrefab;

    private BTNode root;
    private Transform player;
    private Animator animator;
    private BossHealth bossHealth;
    private Rigidbody2D rb;

    private bool playerInRoom = false;
    public bool isPerformingSpecialAttack = false;
    private bool shieldActive = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bossHealth = GetComponent<BossHealth>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        #region Condition Nodes
        var isPlayerInRoom = new ConditionNode(() => playerInRoom);
        var isPlayerInAttack1Range = new ConditionNode(() => Vector2.Distance(transform.position, player.position) < attack1Range);
        var isPlayerInAttack2Range = new ConditionNode(() => Vector2.Distance(transform.position, player.position) < attack2Range);
        var isPlayerInSpecialAttackRange = new ConditionNode(() => Vector2.Distance(transform.position, player.position) < specialAttackRange);
        var isTooFarForAttack = new ConditionNode(() => Vector2.Distance(transform.position, player.position) > attack1Range);
        var isTooFarForSpecialAttack = new ConditionNode(() => Vector2.Distance(transform.position, player.position) > specialAttackRange);

        var isLowHP = new ConditionNode(() => bossHealth.GetHealth() < maxHP * 0.3f);
        var isRageMode = new ConditionNode(() => bossHealth.GetHealth() <= maxHP * 0.15f);

        var isInDefensiveZone = new ConditionNode(() =>
        {
            float hp = bossHealth.GetHealth();
            return hp <= maxHP * 0.3f && hp > maxHP * 0.15f;
        });

        #endregion

        #region Actions
        var attack1 = new MeleeAttackNode(
            transform, player, animator,
            "attack1",
            cooldown: 2f,
            attackDuration: attack1Duration,
            damage: 1f,
            knockbackForce: 5f,
            playerLayer: playerLayer,
            attackRange: attack1Range,
            damageFrameRatio: 3f / 7f
        );

        var attack2 = new MeleeAttackNode(
            transform, player, animator,
            "attack2",
            cooldown: 3f,
            attackDuration: attack2Duration,
            damage: 1.5f,
            knockbackForce: 7f,
            playerLayer: playerLayer,
            attackRange: attack2Range,
            damageFrameRatio: 3f / 7f
        );

        var specialAttack = new SpecialCrystalAttackNode(
            transform, player, animator, crystalPrefab, crystalSpawnPoint,
            cooldown: 3f,
            attackDuration: 2.0f,
            specialAttackRange: specialAttackRange,
            playerLayer: LayerMask.GetMask("Player")
        );

        var activateShield = new ActionNode(() =>
        {
            if (!shieldActive)
            {
                shieldObject.SetActive(true);
                shieldActive = true;
            }
            return NodeState.SUCCESS;
        });

        var deactivateShield = new ActionNode(() =>
        {
            if (shieldObject != null && shieldObject.activeSelf)
            {
                shieldObject.SetActive(false);
                shieldActive = false;
            }

            return NodeState.SUCCESS;
        });

        var heal = new ActionNode(() =>
        {
            bossHealth.Heal(bossHealth.GetMaxHealth() * 0.2f);
            Instantiate(healEffectPrefab, transform.position, Quaternion.identity);
            return NodeState.SUCCESS;
        });

        var chase = new ChaseNode(transform, player, chaseSpeed, animator, rb, obstacleMask);

        var setRageSpeed = new ActionNode(() =>
        {
            chase.SetSpeed(chaseSpeed * 2f); 
            return NodeState.SUCCESS;
        });

        var enterRage = new ActionNode(() =>
        {
            if (bossSprite != null)
                bossSprite.color = rageColor;
            return NodeState.SUCCESS;
        });

        var idle = new ActionNode(() =>
        {
            animator.SetBool("isRunning", false);
            return NodeState.SUCCESS;
        });
        #endregion

        #region Cooldowns
        var crystalCooldown = new CooldownNode(specialAttack, attackCooldownTime);
        var healCooldownNode = new CooldownNode(heal, healCooldown);
        var rageCrystal = new CooldownNode(specialAttack, rageCooldownTime);
        var rageAttack1 = new CooldownNode(attack1, rageCooldownTime);
        var rageAttack2 = new CooldownNode(attack2, rageCooldownTime);
        #endregion

        var approachAndAttack = new Sequence(new List<BTNode>
        {
            new Repeater(
                new Sequence(new List<BTNode>
                {
                    new ConditionNode(() => Vector2.Distance(transform.position, player.position) > specialAttackRange),
                    chase
                }),
                repeatUntilSuccess: true
                ),
            crystalCooldown
        });

        #region Subtrees

        // --- Rage ---

        var rageTree = new Sequence(new List<BTNode>
        {
            isRageMode,
            enterRage,
            setRageSpeed,
            new Repeater(
            new Selector(new List<BTNode>
            {
                new Sequence(new List<BTNode>
                {
                    isPlayerInAttack2Range,
                    rageAttack2
                }),
                new Sequence(new List<BTNode>
                {
                    isPlayerInAttack1Range,
                    rageAttack1
                }),
                new Sequence(new List<BTNode>
                {
                    isPlayerInSpecialAttackRange,
                    rageCrystal
                }),
                chase
            }),
            repeatUntilSuccess: false
            )
        });

        // --- Defensive ---
        var defensiveTree = new Sequence(new List<BTNode>
        {
            isInDefensiveZone,
            activateShield,
            approachAndAttack,
            deactivateShield,
            heal
        });


        var combatTree = new Sequence(new List<BTNode>
        {
            isPlayerInRoom,
            new Selector(new List<BTNode>
            {
                new Sequence(new List<BTNode> { isPlayerInAttack1Range, attack1 }),
                new Sequence(new List<BTNode> { isPlayerInAttack2Range, attack2 }),
                //new Sequence(new List<BTNode> { isPlayerInSpecialAttackRange, crystalCooldown }),
                new Sequence(new List<BTNode> { isTooFarForAttack, chase })
            })
        });

        #endregion

        // --- Final Root Tree ---
        root = new Selector(new List<BTNode>
        {
            rageTree,
            defensiveTree,
            combatTree,
            idle
        });
    }

    void Update()
    {
        btStopwatch.Restart();

        root?.Evaluate();

        btStopwatch.Stop();
        btEvaluationTime = btStopwatch.ElapsedTicks / (float)Stopwatch.Frequency * 1000f;

     //   PerformanceLogger.Instance.LogTime("BT", btEvaluationTime);

        currentHP = bossHealth.GetHealth();
    }

    public float GetHealth()
    {
        return bossHealth.GetHealth();
    }

    public BTNode GetRootNode()
    {
        return root;
    }

    public void SetPlayerInRoom(bool isInRoom)
    {
        playerInRoom = isInRoom;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attack1Range);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attack2Range);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, specialAttackRange);
    }
}