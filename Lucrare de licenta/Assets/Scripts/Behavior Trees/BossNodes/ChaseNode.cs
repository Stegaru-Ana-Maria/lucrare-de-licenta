using UnityEngine;

public class ChaseNode : BTNode
{
    private Transform bossTransform;
    private Transform playerTransform;
    private float speed;
    private Animator animator;
    private float stopDistance = 1.2f;
    private Rigidbody2D rb;
    private LayerMask obstacleMask;
    private float jumpForce = 10f;
    private float obstacleCheckDistance = 2.0f;
    private float groundCheckDistance = 1.5f;

    public void SetSpeed(float chaseSpeed)
    {
        speed = chaseSpeed;
    }

    public ChaseNode(Transform bossTransform, Transform playerTransform, float speed, Animator animator, Rigidbody2D rb, LayerMask obstacleMask)
    {
        this.bossTransform = bossTransform;
        this.playerTransform = playerTransform;
        this.speed = speed;
        this.animator = animator;
        this.rb = rb;
        this.obstacleMask = obstacleMask;
    }

    public override NodeState Evaluate()
    {
        if (playerTransform == null)
        {
            animator.SetBool("isRunning", false);
            return NodeState.FAILURE;
        }

        float distance = Vector2.Distance(bossTransform.position, playerTransform.position);
        if (distance <= stopDistance)
        {
            animator.SetBool("isRunning", false);
            return NodeState.SUCCESS;
        }

        if (bossTransform.GetComponent<Boss2AI>().isPerformingSpecialAttack)
        {
            animator.SetBool("isRunning", false);
            return NodeState.FAILURE;
        }

        animator.SetBool("isRunning", true);
        FlipTowardsPlayer();

        if (IsObstacleAhead() && IsGrounded())
        {
            Jump();
        }

        bossTransform.position = Vector2.MoveTowards(
            bossTransform.position,
            new Vector2(playerTransform.position.x, bossTransform.position.y),
            speed * Time.deltaTime
        );

        return NodeState.RUNNING;
    }

    private void FlipTowardsPlayer()
    {
        if ((playerTransform.position.x < bossTransform.position.x && bossTransform.localScale.x > 0) ||
            (playerTransform.position.x > bossTransform.position.x && bossTransform.localScale.x < 0))
        {
            bossTransform.localScale = new Vector3(-bossTransform.localScale.x, bossTransform.localScale.y, bossTransform.localScale.z);
        }
    }

    private bool IsObstacleAhead()
    {
        Vector2 direction = bossTransform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 origin = new Vector2(bossTransform.position.x, bossTransform.position.y + 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, obstacleCheckDistance, obstacleMask);

        Debug.DrawRay(origin, direction * obstacleCheckDistance, Color.red);

        return hit.collider != null;
    }

    private bool IsGrounded()
    {
        Vector2 origin = new Vector2(bossTransform.position.x, bossTransform.position.y - 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, obstacleMask);

        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, Color.yellow);

        return hit.collider != null;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        animator.SetTrigger("jump");
        SoundEffectManager.Play("EnemyJump");
    }
}