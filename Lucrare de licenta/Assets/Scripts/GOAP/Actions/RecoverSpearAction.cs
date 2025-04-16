using UnityEngine;

public class RecoverSpearAction : GOAPAction
{
    private GameObject spear;
    private bool recovered = false;

    public RecoverSpearAction()
    {
        actionName = "Recover Spear";
        cost = 2f;

        AddPrecondition("spearOnGround", true);
        AddEffect("hasSpear", true);
        AddEffect("spearOnGround", false);
    }

    public override void ResetAction()
    {
        recovered = false;
        spear = null;
    }

    public override bool IsDone()
    {
        return recovered;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        Debug.Log("Checking procedural preconditions for RecoverSpearAction...");
        Spear[] allSpears = GameObject.FindObjectsByType<Spear>(FindObjectsSortMode.None);
        Debug.Log($"[RecoverSpearAction] Found {allSpears.Length} spears");

        float closestDist = Mathf.Infinity;
        GameObject closestSpear = null;

        foreach (Spear s in allSpears)
        {
            if (s.IsRecoverable())
            {
                float dist = Vector3.Distance(agent.transform.position, s.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestSpear = s.gameObject;
                }
            }
        }

        if (closestSpear != null)
        {
            spear = closestSpear;
            target = spear;
            Debug.Log("Found recoverable spear at: " + spear.transform.position);
            return true;
        }

        Debug.LogWarning("No recoverable spear found.");
        return false;
    }

    public override bool Perform(GameObject agent)
    {
        Debug.Log("Performing RecoverSpearAction");

        if (spear == null)
        {
            Debug.LogError("No spear assigned in RecoverSpearAction!");
            return false;
        }

        float dist = Vector3.Distance(agent.transform.position, spear.transform.position);
        Debug.Log($"[RecoverSpearAction] Distance to spear: {dist}");

        if (dist < 1.5f)
        {
            Debug.Log("Spear reached. Recovering spear...");

            // Simuleaza recuperarea sulitei
            Object.Destroy(spear);
            recovered = true;

            GOAPAgent goap = agent.GetComponent<GOAPAgent>();
            if (goap != null)
            {
                goap.SetHasSpear(true);
                goap.worldState.SetState("hasSpear", true);
                goap.worldState.SetState("spearOnGround", false);
                Debug.Log("GOAPAgent updated with hasSpear=true and spearOnGround=false");
            }

            return true;
        }

        // Se apropie de sulita
        agent.GetComponent<UnityEngine.AI.NavMeshAgent>()?.SetDestination(spear.transform.position);
        Debug.Log("Approaching spear at: " + spear.transform.position);

        return true;
    }
}
