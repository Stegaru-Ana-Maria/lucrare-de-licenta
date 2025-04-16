using UnityEngine;

public class RunToPlayerAction : GOAPAction
{
    private Transform bossTransform;
    private Transform playerTransform;
    private Animator animator;
    private Rigidbody2D rb;
    private LayerMask obstacleMask;

    private float speed = 5f;
    private float stopDistance = 3.5f;
    private float obstacleCheckDistance = 2.0f;
    private float groundCheckDistance = 1.5f;
    private float jumpForce = 8f;

    private bool isDone = false;

    protected override void Awake()
    {
        base.Awake();
        AddPrecondition("playerVisible", true);
        AddEffect("playerInRange", true);
    }

    protected override void Start()
    {
        base.Start();
        bossTransform = transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        obstacleMask = LayerMask.GetMask("Ground"); 
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            target = player;
            return true;
        }
        return false;
    }

    public override bool Perform(GameObject agent)
    {
        if (playerTransform == null)
        {
            animator.SetBool("isRunning", false);
            isDone = true;
            return false;
        }

        float distance = Vector2.Distance(bossTransform.position, playerTransform.position);
        if (distance <= stopDistance)
        {
            animator.SetBool("isRunning", false);
            isDone = true;
            return true;
        }

        animator.SetBool("isRunning", true);
        FlipTowardsPlayer();

        if (IsObstacleAhead() && IsGrounded())
        {
            Jump();
        }

        Vector2 targetPosition = new Vector2(playerTransform.position.x, bossTransform.position.y);
        bossTransform.position = Vector2.MoveTowards(bossTransform.position, targetPosition, speed * Time.deltaTime);

        return true;
    }

    public override bool IsDone()
    {
        return isDone;
    }

    public override void ResetAction()
    {
        isDone = false;
        animator.SetBool("isRunning", false);
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
    }
}
