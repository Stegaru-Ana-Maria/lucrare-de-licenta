using UnityEngine;

public class ThrowSpearAction : GOAPAction
{
    private BossSpearThrow spearThrower;
    private Transform playerTarget;
    private bool spearThrown = false;

    public ThrowSpearAction()
    {
        actionName = "Throw Spear";
        cost = 3f;

        AddPrecondition("playerVisible", true);
        AddPrecondition("hasSpear", true);

        AddEffect("damagePlayer", true);
        AddEffect("hasSpear", false); 
    }

    public override void ResetAction()
    {
        spearThrown = false;
    }

    public override bool IsDone()
    {
        return spearThrown;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        Debug.Log("Checking preconditions for ThrowSpearAction");

        if (spearThrower == null)
            spearThrower = agent.GetComponent<BossSpearThrow>();

        if (playerTarget == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                playerTarget = playerObj.transform;
        }

        return spearThrower != null && playerTarget != null;
        // float dist = Vector3.Distance(transform.position, playerTarget.position);
        //  return dist <= 10f;
    }

    public override bool Perform(GameObject agent)
    {
        if (spearThrown || spearThrower == null || playerTarget == null)
            return false;

        spearThrower.SetTarget(playerTarget);

        if (spearThrower.TryShoot())
        {
            Debug.Log("Performing ThrowSpear");

            spearThrown = true;

            GOAPAgent goap = agent.GetComponent<GOAPAgent>();
            if (goap != null)
            {
                goap.worldState.SetState("hasSpear", false);
                goap.worldState.SetState("spearOnGround", true);
            }

            return true;
        }

        return false;
    }
}
