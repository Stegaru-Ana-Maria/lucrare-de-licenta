using UnityEngine;

public class Attack1Action : GOAPAction
{
    private float damage = 1f;
    private float attackRange = 3f;
    private float cooldown = 2f;
    private float knockbackForce = 5f;
    private float attackDuration = 0.7f;
    private float damageFrameRatio = 4f / 5f;

    private float lastAttackTime = -999f;
    private float attackStartTime;
    private bool hasDealtDamage = false;
    private bool isAttacking = false;

    [SerializeField] private LayerMask playerLayer;
    private Animator animator;
    private Transform player;
    private Transform self;

    public Attack1Action()
    {
        AddPrecondition("playerInAttack1Range", true);
        AddPrecondition("hasSpear", true);
        AddEffect("damagePlayer", true);
        cost = 1f; 
    }
    public override void ResetAction()
    {
        Debug.Log("Reset Attack1Action");
        isAttacking = false;
        hasDealtDamage = false;
        attackStartTime = 0f;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        self = agent.transform;
        var goap = agent.GetComponent<GOAPAgent>();
        player = goap.GetPlayerTransform();
        animator = agent.GetComponent<Animator>();
        target = player.gameObject;
        if (player == null)
        {
            Debug.LogWarning("Attack1Action: player not found!");
            return false;
        }

        Debug.Log("Attack1Action: player found and in range check passed");
        return true;
    }

    public override bool IsDone()
    {
        return !isAttacking;
    }

    public override bool Perform(GameObject agent)
    {
        // Daca playerul nu mai e in range in timpul atacului, intrerupe actiunea
        if (!CheckProceduralPrecondition(agent))
        {
            Debug.LogWarning("Attack1Action: lost track of player, aborting action.");
            isAttacking = false;
            return false; // forteaza replanificarea
        }

        float now = Time.time;

        if (!isAttacking)
        {
            if (now < lastAttackTime + cooldown)
            {
                Debug.Log($"Attack1Action: cooldown active. {now} < {lastAttackTime + cooldown}");
                return false;
            }

            Debug.Log("Attack1Action: starting attack!");
            animator.SetTrigger("attack1");
            attackStartTime = now;
            hasDealtDamage = false;
            isAttacking = true;
            return true;
        }

        float elapsed = now - attackStartTime;

        if (!hasDealtDamage && elapsed >= attackDuration * damageFrameRatio)
        {
            Debug.Log("Attack1Action: checking for hit on player...");

            Collider2D hit = Physics2D.OverlapCircle(self.position, attackRange, playerLayer);
            if (hit != null && hit.TryGetComponent(out Health hp))
            {
                Debug.Log("Attack1Action: HIT! Dealing damage to player.");
                hp.TakeDamage(damage);
                Vector2 knockDir = (hit.transform.position - self.position).normalized;
                hp.ApplyKnockback(knockDir * knockbackForce);
                hasDealtDamage = true;
            }
        }

        if (elapsed >= attackDuration)
        {
            Debug.Log("Attack1Action: attack finished.");
            lastAttackTime = now;
            isAttacking = false;
        }

        return true;
    }
}
