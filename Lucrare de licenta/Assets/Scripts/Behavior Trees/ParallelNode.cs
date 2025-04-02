using System.Collections.Generic;

public class ParallelNode : BTNode
{
    protected List<BTNode> nodes;
    private int successThreshold;

    public ParallelNode(List<BTNode> nodes, int successThreshold)
    {
        this.nodes = nodes;
        this.successThreshold = successThreshold;
    }

    public override NodeState Evaluate()
    {
        int successCount = 0;
        int failureCount = 0;

        foreach (var node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.SUCCESS:
                    successCount++;
                    break;
                case NodeState.FAILURE:
                    failureCount++;
                    break;
            }
        }

        if (successCount >= successThreshold)
        {
            _nodeState = NodeState.SUCCESS;
        }
        else if (failureCount > nodes.Count - successThreshold)
        {
            _nodeState = NodeState.FAILURE;
        }
        else
        {
            _nodeState = NodeState.RUNNING;
        }

        return _nodeState;
    }
}