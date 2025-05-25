using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    private EnemyState currentState;

    [Header("General Settings")]
    public Transform player;
    public Animator anim;
    public Transform enemy;
    public LayerMask playerLayer;
    public Collider2D enemyCollider;
    public float verticalAttackTolerance = 2.5f;
    public float horizontalChaseTolerance = 0.5f;
    public float maxVerticalChaseDistance = 3.5f;

    [Header("Attack")]
    [SerializeField] public float attackRange = 3.5f;
    [SerializeField] public int damage = 1;
    [SerializeField] public float attackCooldown = 2f;
    [SerializeField] public float knockbackForce = 7f;

    [Header("Patrolling")]
    public Transform patrolPointA;
    public Transform patrolPointB;
    [SerializeField] public float patrolSpeed = 4f;
    [SerializeField] public float idleDuration = 2f;

    [Header("Chasing")]
    [SerializeField] public float chaseSpeed = 6f;
    [SerializeField] public float detectionRange = 10f;
    [SerializeField] public float stopChaseDistance = 15f;
    public LayerMask obstacleLayer;

    private void Start()
    {
        ChangeState(new PatrolState(this)); 
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void ChangeState(EnemyState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public void DamagePlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(enemy.position, attackRange, playerLayer);

        if (hit != null)
        {
            Health playerHealth = hit.GetComponent<Health>();
            if (playerHealth != null)
            {
                Debug.Log("Enemy hit the player!");
                playerHealth.TakeDamage(damage);

                Vector2 knockbackDirection = (hit.transform.position - enemy.position).normalized;
                playerHealth.ApplyKnockback(knockbackDirection * knockbackForce);
            }
        }
        else
        {
            Debug.Log("Attack missed. No player in range.");
        }
    }

    public string GetCurrentStateName()
    {
        if (currentState != null)
            return currentState.GetStateName();
        else
            return "No State";
    }

    public void ForceChaseState()
    {
        ChangeState(new ChaseState(this));
        Debug.Log("Enemy forced into Chase State");
    }

    public void ForceAttackState()
    {
        ChangeState(new AttackState(this));
        Debug.Log("Enemy forced into Attack State");
    }

}
