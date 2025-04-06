using UnityEngine;

public class RetreatNode : BTNode
{
    private Transform boss;
    private Transform player;
    private Rigidbody2D rb;
    private float retreatSpeed;
    private float retreatDistance;

    public RetreatNode(Transform boss, Transform player, Rigidbody2D rb, float retreatSpeed, float retreatDistance)
    {
        this.boss = boss;
        this.player = player;
        this.rb = rb;
        this.retreatSpeed = retreatSpeed;
        this.retreatDistance = retreatDistance;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector2.Distance(boss.position, player.position);

        if (distance < retreatDistance)
        {
            Vector2 direction = (boss.position - player.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * retreatSpeed, rb.linearVelocity.y);

            return NodeState.RUNNING;
        }

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y); // se opreste daca a ajuns destul de departe
        return NodeState.SUCCESS;
    }
}

