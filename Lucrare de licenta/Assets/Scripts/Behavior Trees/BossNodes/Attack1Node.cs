using UnityEngine;

public class Attack1Node : BTNode
{
    private readonly Transform boss;
    private readonly Transform player;
    private readonly Animator animator;

    private readonly float cooldown;
    private readonly float attackDuration;
    private readonly float damage;
    private readonly float knockbackForce;
    private readonly LayerMask playerLayer;
    private readonly float attackRange;

    private float lastAttackTime = -999f;
    private bool isAttacking = false;
    private bool hasDealtDamage = false;
    private float attackStartTime;

    private const float damageFrameRatio = 3f / 7f;

    public Attack1Node(Transform boss, Transform player, Animator animator,
                       float cooldown, float attackDuration, float damage,
                       float knockbackForce, LayerMask playerLayer, float attackRange)
    {
        this.boss = boss;
        this.player = player;
        this.animator = animator;
        this.cooldown = cooldown;
        this.attackDuration = attackDuration;
        this.damage = damage;
        this.knockbackForce = knockbackForce;
        this.playerLayer = playerLayer;
        this.attackRange = attackRange;
    }

    public override NodeState Evaluate()
    {
        float currentTime = Time.time;

        if (currentTime < lastAttackTime + cooldown)
            return NodeState.FAILURE;

        if (!isAttacking)
        {
            animator.SetTrigger("attack1");
            attackStartTime = currentTime;
            hasDealtDamage = false;
            isAttacking = true;
            return NodeState.RUNNING;
        }

        float elapsed = currentTime - attackStartTime;

        if (!hasDealtDamage && elapsed >= attackDuration * damageFrameRatio)
        {
            TryDealDamage();
            hasDealtDamage = true;
        }

        if (elapsed >= attackDuration)
        {
            lastAttackTime = currentTime;
            isAttacking = false;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }

    private void TryDealDamage()
    {
        Collider2D hit = Physics2D.OverlapCircle(boss.position, attackRange, playerLayer);
        if (hit == null) return;

        Health targetHealth = hit.GetComponent<Health>();
        if (targetHealth == null) return;

        targetHealth.TakeDamage(damage);
        Vector2 knockbackDirection = (hit.transform.position - boss.position).normalized;
        targetHealth.ApplyKnockback(knockbackDirection * knockbackForce);
    }
}
