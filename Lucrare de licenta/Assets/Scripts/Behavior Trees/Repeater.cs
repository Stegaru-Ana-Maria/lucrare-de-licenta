public class Repeater : BTNode
{
    private BTNode node;
    private int repeatCount;
    private int currentCount;
    private bool repeatUntilSuccess;

    public Repeater(BTNode node, int repeatCount = -1, bool repeatUntilSuccess = false)
    {
        this.node = node;
        this.repeatCount = repeatCount;
        this.currentCount = 0;
        this.repeatUntilSuccess = repeatUntilSuccess;
    }

    public override NodeState Evaluate()
    {
        if (repeatCount > 0 && currentCount >= repeatCount)
        {
            _nodeState = NodeState.SUCCESS;
            return _nodeState;
        }

        NodeState result = node.Evaluate();

        if (repeatUntilSuccess)
        {
            if (result == NodeState.SUCCESS)
            {
                _nodeState = NodeState.SUCCESS;
                return _nodeState;
            }
        }
        else
        {
            if (result == NodeState.FAILURE || result == NodeState.SUCCESS)
            {
                currentCount++;
            }

            if (repeatCount > 0 && currentCount >= repeatCount)
            {
                _nodeState = NodeState.SUCCESS;
                return _nodeState;
            }
        }

        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }
}
