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
    [SerializeField] public float attackCooldown = 2f;

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
        if (collision.CompareTag("Player") && !(currentState is FlyingAttackState))
        {
            Debug.Log("Player detectat - Trecerea in starea de atac");
            ChangeState(new FlyingAttackState(this));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}