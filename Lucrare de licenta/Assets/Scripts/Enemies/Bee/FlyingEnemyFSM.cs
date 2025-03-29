using UnityEditor;
using UnityEngine;

public class FlyingEnemyFSM : MonoBehaviour
{
    private FlyingEnemyState currentState;

    [Header("General Settings")]
    public Transform player;
    public Animator anim;
    public Transform enemy;
    public LayerMask playerLayer;
    private float lastAttackTime = 5f;
    [SerializeField] public float detectionRange = 10f;
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
    [SerializeField] public float stopChaseDistance = 15f;

    private void Start()
    {
        ChangeState(new FlyingPatrolState(this));
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void ChangeState(FlyingEnemyState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Time.time >= lastAttackTime + attackCooldown)
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                ApplyKnockback(collision.transform);
                lastAttackTime = Time.time;
            }
        }
    }

    private void ApplyKnockback(Transform playerTransform)
    {
        Rigidbody2D playerRb = playerTransform.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 knockbackDirection = (playerTransform.position - transform.position).normalized;

            float horizontalForce = knockbackForce * Mathf.Sign(knockbackDirection.x);
            float verticalForce = knockbackForce * 0.5f;

            Vector2 knockbackVector = new Vector2(horizontalForce, verticalForce);

            playerRb.linearVelocity = Vector2.zero;
            playerRb.AddForce(knockbackVector, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}