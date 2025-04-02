using UnityEngine;

public class Repeater : BTNode
{
    private BTNode node;
    private int repeatCount;
    private int currentCount;

    public Repeater(BTNode node, int repeatCount = -1) // -1 inseamna infinit
    {
        this.node = node;
        this.repeatCount = repeatCount;
        this.currentCount = 0;
    }

    public override NodeState Evaluate()
    {
        if (repeatCount > 0 && currentCount >= repeatCount)
        {
            _nodeState = NodeState.SUCCESS;
            return _nodeState;
        }

        NodeState result = node.Evaluate();
        if (result == NodeState.SUCCESS || result == NodeState.FAILURE)
        {
            currentCount++;
        }

        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }
}