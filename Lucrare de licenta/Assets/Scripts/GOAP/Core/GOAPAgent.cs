using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GOAPAgent : MonoBehaviour
{
    private GOAPPlanner planner;
    private Queue<GOAPAction> actionQueue;
    private GOAPAction currentAction;
    [SerializeField] private Spear spear;

    public WorldState worldState { get; private set; } = new WorldState();
    public Goal currentGoal { get; private set; } = new Goal();

    [Header("Agent Stats")]
    [SerializeField] private float health = 10f;
    [SerializeField] private bool hasSpear = true;

    [Header("Target References")]
    [SerializeField] private Transform playerTransform;

    private void Start()
    {
        planner = new GOAPPlanner();

        // Initializare world state (doar ca exemplu, vei actualiza constant in runtime)
        worldState.SetState("hasSpear", hasSpear);
        worldState.SetState("playerVisible", false);

        currentGoal.SetGoal("playerInRange", true);
        //currentGoal.SetGoal("damagePlayer", true);
    }

    private void Update()
    {
        UpdateWorldState();

        if (actionQueue == null || actionQueue.Count == 0 || currentAction == null || !currentAction.CheckProceduralPrecondition(gameObject))
        {
            var actions = new List<GOAPAction>(GetComponents<GOAPAction>());
            //var plan = planner.Plan(actions, worldState.GetAllStates(), currentGoal.GetGoalState());
            var plan = planner.Plan(gameObject, actions, worldState.GetAllStates(), currentGoal.GetGoalState());

            // Resetam actiunea curenta (daca exista)
            if (currentAction != null)
            {
                currentAction.ResetAction();
                currentAction = null;
            }

            if (plan != null)
            {
                actionQueue = plan;
                Debug.Log("Plan gasit: " + string.Join(" -> ", actionQueue.Select(a => a.GetType().Name)));
                currentAction = null;
                //Debug.Log("Plan gasit: " + string.Join(" -> ", actionQueue));
                // Debug.Log("Plan gasit: " + string.Join(" -> ", actionQueue.Select(a => a.GetType().Name)));
            }
        }

        if (actionQueue != null && actionQueue.Count > 0)
        {
            if (currentAction == null)
            {
                currentAction = actionQueue.Peek();
            }

            if (!currentAction.IsDone())
            {
                // Verifica din nou preconditia procedurala în caz ca s-a schimbat intre timp
                if (currentAction.CheckProceduralPrecondition(gameObject))
                {
                    Debug.Log("Execut actiunea: " + currentAction.GetType().Name);
                    currentAction.Perform(gameObject);
                }
                else
                {
                    // Daca preconditia nu mai e valida, resetam planul
                    currentAction.ResetAction();
                    currentAction = null;
                    actionQueue.Clear(); // reconstruieste planul în urmatorul frame
                }
            }
            else
            {
                currentAction.ResetAction();
                actionQueue.Dequeue();
                currentAction = null;
            }
        }
    }

    private void UpdateWorldState()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Exemplu de actualizare a starii (trebuie sa implementezi metodele de mai jos)
        worldState.SetState("playerVisible", SeePlayer());
        worldState.SetState("playerInAttack1Range", CheckAttack1Range());
        worldState.SetState("playerInAttack2Range", CheckAttack2Range());
        worldState.SetState("spearOnGround", CheckSpearOnGround());
        worldState.SetState("hasLowHealth", health < 3f);
        worldState.SetState("crystalNearby", CheckNearbyCrystal());
        worldState.SetState("obstacleAhead", CheckObstacle());
        worldState.SetState("canReachPlayer", CanReachPlayer());
        worldState.SetState("distanceFromPlayer", distanceToPlayer > 15f);
    }

    private bool CanReachPlayer()
    {
        return SeePlayer() && !CheckObstacle();
    }

    // Aceste metode ar trebui implementate conform jocului tau
    private bool SeePlayer()
    {
        if (playerTransform == null) return false;

        float visionRange = 20f; 
        return Vector2.Distance(transform.position, playerTransform.position) <= visionRange;
    }

    private bool CheckAttack1Range()
    {
        if (playerTransform == null) return false;

        float attack1Range = 3f;
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        return distance <= attack1Range;
    }

    private bool CheckAttack2Range()
    {
        if (playerTransform == null) return false;

        float attack2Range = 4f;
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        return distance <= attack2Range;
    }

    private bool CheckSpearOnGround()
    {
        return spear != null && spear.HasHitSomething();
    }


    private bool CheckSpearOnGround1()
    {
        GameObject[] spears = GameObject.FindGameObjectsWithTag("Spear");
        foreach (var spear in spears)
        {
            var s = spear.GetComponent<Spear>();
            if (s != null && s.HasHitSomething())
                return true;
        }
        return false;
    }

    private bool CheckNearbyCrystal()
    {
        GameObject[] crystals = GameObject.FindGameObjectsWithTag("Crystal");
        float detectionRadius = 5f;
        foreach (var crystal in crystals)
        {
            if (Vector2.Distance(transform.position, crystal.transform.position) <= detectionRadius)
                return true;
        }
        return false;
    }

    private bool CheckObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (playerTransform != null)
            Gizmos.DrawLine(transform.position, playerTransform.position);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 5f); // crystal detection radius
    }


    // Metoda publica pentru a actualiza spear-ul (folosit de actiuni)
    public void SetHasSpear(bool value) => hasSpear = value;

    public void SetHealth(float value) => health = value;

    public Transform GetPlayerTransform() => playerTransform;

    public bool HasSpear() => hasSpear;
    public float GetHealth() => health;
    public void ModifyHealth(float delta) => health += delta;

}
