using UnityEngine;

public class BossHealthNode : BTNode
{
    private BossHealth bossHealth;
    private float threshold;

    public BossHealthNode(BossHealth bossHealth, float threshold)
    {
        this.bossHealth = bossHealth;
        this.threshold = threshold;
    }

    public override NodeState Evaluate()
    {
        return bossHealth.GetHealth() < threshold ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}