using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class FlyingEnemyFSM : MonoBehaviour
{
    private Stopwatch stopwatch;

    private FlyingEnemyState currentState;

    [Header("General Settings")]
    public Transform player;
    public Animator anim;
    public Transform enemy;
    public LayerMask playerLayer;

    [Header("Attack")]
    private float lastAttackTime = 5f;
    [SerializeField] public float attackCooldown = 2f;

    [Header("Patrolling")]
    public Transform patrolPointA;
    public Transform patrolPointB;
    [SerializeField] public float patrolSpeed = 4f;
    [SerializeField] public float idleDuration = 2f;

    [Header("Chasing")]
    [SerializeField] public float chaseSpeed = 6f;
    [SerializeField] public float detectionRange = 10f;
    [SerializeField] public float stopChaseDistance = 15f;

    private void Start()
    {
        stopwatch = new Stopwatch();
        ChangeState(new FlyingPatrolState(this));
    }

    private void Update()
    {
        stopwatch.Reset();
        stopwatch.Start();

        currentState?.UpdateState();

        stopwatch.Stop();
        float elapsedTime = stopwatch.ElapsedTicks / (float)Stopwatch.Frequency * 1000f;

      //  PerformanceLogger.Instance.LogTime("Pathfinding", elapsedTime);
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
            ChangeState(new FlyingAttackState(this));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}