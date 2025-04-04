using UnityEngine;
using System.Collections.Generic;

public class Boss2AI : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float bossHP;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Ranges")]
    [SerializeField] private float attack1Range = 3f;
    [SerializeField] private float attack2Range = 2.5f;
    [SerializeField] private float specialAttackRange = 10f;

    [Header("Cooldowns")]
    [SerializeField] private float attackCooldownTime = 2f;
    [SerializeField] private float healCooldown = 10f;
    [SerializeField] private float berserkCooldownTime = 1f;

    [Header("Dependencies")]
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private GameObject attack1Hitbox;
    [SerializeField] private float attack1Duration = 0.5f;


    private BTNode root;
    private Transform player;
    private Animator animator;
    private BossHealth bossHealth;
    private Rigidbody2D rb;

    private bool playerInRoom = false;
    private bool isBerserk = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bossHealth = GetComponent<BossHealth>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        bossHP = maxHP;

        #region Condition Nodes
        var isPlayerInRoom = new ConditionNode(() => playerInRoom);
        var isPlayerInAttack1Range = new ConditionNode(() => Vector2.Distance(transform.position, player.position) < attack1Range);
        var isPlayerInAttack2Range = new ConditionNode(() => Vector2.Distance(transform.position, player.position) < attack2Range);
        var isPlayerInLaserRange = new ConditionNode(() => Vector2.Distance(transform.position, player.position) < specialAttackRange);
        var isTooFarForAttack = new ConditionNode(() => Vector2.Distance(transform.position, player.position) > attack1Range);

        var isLowHP = new ConditionNode(() => bossHP < maxHP * 0.3f);
        var isBerserk = new ConditionNode(() => bossHP < maxHP * 0.15f);
        #endregion

        #region Actions
        var attack1 = new Attack1Node(transform, player, animator, attackCooldownTime, attack1Duration, 1f, 8f, LayerMask.GetMask("Player"), attack1Range);


        var attack2 = new ActionNode(() =>
        {
            animator.SetTrigger("Attack2");
            Debug.Log("Boss foloseste Attack2");
            return NodeState.SUCCESS;
        });

        var specialAttack = new ActionNode(() =>
        {
            animator.SetTrigger("Laser");
            Debug.Log("Boss lanseaza atacul special (Laser)");
            return NodeState.SUCCESS;
        });

        var heal = new ActionNode(() =>
        {
            Debug.Log("Boss se vindeca!");
            bossHP += maxHP * 0.2f;
            bossHP = Mathf.Min(bossHP, maxHP);
            return NodeState.SUCCESS;
        });

        var chase = new ChaseNode(transform, player, chaseSpeed, animator, rb, obstacleMask);

        var idle = new ActionNode(() =>
        {
            animator.SetBool("isRunning", false);
            return NodeState.SUCCESS;
        });
        #endregion

        #region Cooldowns
        var laserCooldown = new CooldownNode(specialAttack, attackCooldownTime);
        var healCooldownNode = new CooldownNode(heal, healCooldown);
        var berserkLaser = new CooldownNode(specialAttack, berserkCooldownTime);
        var berserkAttack1 = new CooldownNode(attack1, berserkCooldownTime);
        var berserkAttack2 = new CooldownNode(attack2, berserkCooldownTime);
        #endregion

        #region Subtrees

        // --- Berserk ---
        var berserkAttack = new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode> { isPlayerInAttack1Range, berserkAttack1 }),
            new Sequence(new List<BTNode> { isPlayerInAttack2Range, berserkAttack2 }),
            new Sequence(new List<BTNode> { isPlayerInLaserRange, berserkLaser })
        });

        var berserkTree = new Sequence(new List<BTNode>
        {
            isBerserk,
            berserkAttack
        });

        // --- Defensive ---
        var defensiveTree = new Sequence(new List<BTNode>
        {
            isLowHP,
            isPlayerInLaserRange,
            laserCooldown,
            healCooldownNode
        });

        // --- Normal Combat ---
        var meleeAttackSequence = new Sequence(new List<BTNode>
        {
            isPlayerInAttack1Range,
            attack1
        });

        var chaseIfTooFar = new Sequence(new List<BTNode>
        {
            isTooFarForAttack,
            chase
        });

        var combatTree = new Sequence(new List<BTNode>
        {
            isPlayerInRoom,
            new Selector(new List<BTNode>
            {
                meleeAttackSequence,
                new Sequence(new List<BTNode> { isPlayerInLaserRange, laserCooldown }),
                chaseIfTooFar
            })
        });

        #endregion

        // --- Final Root Tree ---
        root = new Selector(new List<BTNode>
        {
            berserkTree,
            defensiveTree,
            combatTree,
            idle
        });
    }

    void Update()
    {
        root?.Evaluate();
    }

    public float GetHealth()
    {
        return bossHealth.GetHealth();
    }

    public void SetPlayerInRoom(bool isInRoom)
    {
        playerInRoom = isInRoom;
    }

    public void TakeDamage(float damage)
    {
        bossHP -= damage;
        Debug.Log($"Boss HP: {bossHP}/{maxHP}");
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