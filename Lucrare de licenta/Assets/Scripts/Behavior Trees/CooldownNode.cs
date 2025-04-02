using UnityEngine;

public class CooldownNode : BTNode
{
    private BTNode node;
    private float cooldownTime;
    private float lastTimeExecuted;

    public CooldownNode(BTNode node, float cooldownTime)
    {
        this.node = node;
        this.cooldownTime = cooldownTime;
        this.lastTimeExecuted = -cooldownTime;
    }

    public override NodeState Evaluate()
    {
        if (Time.time - lastTimeExecuted < cooldownTime)
        {
            _nodeState = NodeState.FAILURE;
            return _nodeState;
        }

        NodeState result = node.Evaluate();
        if (result == NodeState.SUCCESS)
        {
            lastTimeExecuted = Time.time;
        }

        _nodeState = result;
        return _nodeState;
    }
}