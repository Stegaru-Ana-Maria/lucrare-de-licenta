using UnityEngine;
using System.Collections.Generic;
using System.Collections;
//using UnityEditor.Experimental.GraphView;

public class Boss2AI : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHP = 10f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Ranges")]
    [SerializeField] private float attack1Range = 3.5f;
    [SerializeField] private float attack2Range = 3f;
    [SerializeField] private float specialAttackRange = 7f;

    [Header("Cooldowns")]
    [SerializeField] private float attackCooldownTime = 2f;
    [SerializeField] private float healCooldown = 10f;
    [SerializeField] private float berserkCooldownTime = 1f;

    [Header("Dependencies")]
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attack1Duration = 0.5f;
    [SerializeField] private float attack2Duration = 1.6f;

    [Header("Special Attack - Crystal")]
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private Transform crystalSpawnPoint;

    private BTNode root;
    private Transform player;
    private Animator animator;
    private BossHealth bossHealth;
    private Rigidbody2D rb;

    private bool playerInRoom = false;
    public bool isPerformingSpecialAttack = false;


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

        var isLowHP = new ConditionNode(() => bossHealth.GetHealth() < maxHP * 0.3f);
        var isBerserk = new ConditionNode(() => bossHealth.GetHealth() < maxHP * 0.15f);
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
            attackRange: attack1Range
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
            cooldown: 8f,
            attackDuration: 2.0f,
            specialAttackRange: specialAttackRange,
            playerLayer: LayerMask.GetMask("Player")
        );

        var retreat = new RetreatNode(transform, player, rb, chaseSpeed * 1.2f, attack1Range + 1.5f);

        var heal = new ActionNode(() =>
        {
            bossHealth.Heal(bossHealth.GetMaxHealth() * 0.2f);
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
        var crystalCooldown = new CooldownNode(specialAttack, attackCooldownTime);
        var healCooldownNode = new CooldownNode(heal, healCooldown);
        var berserkCrystal = new CooldownNode(specialAttack, berserkCooldownTime);
        var berserkAttack1 = new CooldownNode(attack1, berserkCooldownTime);
        var berserkAttack2 = new CooldownNode(attack2, berserkCooldownTime);
        #endregion

        #region Subtrees

        // --- Berserk ---
        var berserkAttack = new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode> { isPlayerInAttack1Range, berserkAttack1 }),
            new Sequence(new List<BTNode> { isPlayerInAttack2Range, berserkAttack2 }),
            new Sequence(new List<BTNode> { isPlayerInSpecialAttackRange, berserkCrystal })
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
             new Selector(new List<BTNode>
             {
                 // Daca e prea aproape, se retrage
                new Sequence(new List<BTNode>{new ConditionNode(() => Vector2.Distance(transform.position, player.position) < attack1Range), retreat}),
                // Daca e la distanta si player-ul e in range, ataca
                new Sequence(new List<BTNode>{isPlayerInSpecialAttackRange, crystalCooldown}),healCooldownNode,idle})
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

    private IEnumerator HandleCrystalAttack()
    {
        // 1. Instantiere cristal
        GameObject crystal = Instantiate(crystalPrefab, crystalSpawnPoint.position, Quaternion.identity);

        // 2. Asteapta pana la frame-ul 12 (~0.13s pe frame * 4 frame-uri = 0.5s)
        yield return new WaitForSeconds(0.5f); // ajusteaza in functie de framerate-ul animatiei bossului

        // 3. Activeaza animatia de "disappear"
        Animator crystalAnim = crystal.GetComponent<Animator>();
        if (crystalAnim != null)
        {
            crystalAnim.SetTrigger("disappear");
        }

        // 4. Asteapta durata animatiei de disparitie (ex: 0.5s)
        yield return new WaitForSeconds(0.5f);

        // 5. Distruge cristalul
        Destroy(crystal);
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