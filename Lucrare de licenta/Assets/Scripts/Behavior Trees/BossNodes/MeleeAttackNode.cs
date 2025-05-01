using UnityEngine;

public class MeleeAttackNode : BTNode
{
    private readonly Transform boss;
    private readonly Transform player;
    private readonly Animator animator;
    private readonly string animationTrigger;
    private readonly float cooldown;
    private readonly float attackDuration;
    private readonly float damage;
    private readonly float knockbackForce;
    private readonly LayerMask playerLayer;
    private readonly float attackRange;
    private readonly float damageFrameRatio;

    private float lastAttackTime = -999f;
    private float attackStartTime;
    private bool isAttacking = false;
    private bool hasDealtDamage = false;

    public MeleeAttackNode(Transform boss, Transform player, Animator animator, string animationTrigger, float cooldown, float attackDuration, float damage, float knockbackForce, LayerMask playerLayer, float attackRange, float damageFrameRatio = 3f / 7f)
    {
        this.boss = boss;
        this.player = player;
        this.animator = animator;
        this.animationTrigger = animationTrigger;
        this.cooldown = cooldown;
        this.attackDuration = attackDuration;
        this.damage = damage;
        this.knockbackForce = knockbackForce;
        this.playerLayer = playerLayer;
        this.attackRange = attackRange;
        this.damageFrameRatio = damageFrameRatio;
    }

    public override NodeState Evaluate()
    {
        float now = Time.time;

        if (!isAttacking)
        {
            if (now < lastAttackTime + cooldown)
                return NodeState.FAILURE;

            StartAttack(now);
            return NodeState.RUNNING;
        }

        float elapsed = now - attackStartTime;
        if (!hasDealtDamage && elapsed >= attackDuration * damageFrameRatio)
            TryDealDamage();

        if (elapsed >= attackDuration)
        {
            EndAttack(now);
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }

    private void StartAttack(float time)
    {
        SoundEffectManager.Play("Hammer");
        animator.SetTrigger(animationTrigger);
        attackStartTime = time;
        hasDealtDamage = false;
        isAttacking = true;
    }

    private void TryDealDamage()
    {
        Collider2D hit = Physics2D.OverlapCircle(boss.position, attackRange, playerLayer);
        if (hit == null) return;

        if (hit.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
            Vector2 knockDir = (hit.transform.position - boss.position).normalized;
            health.ApplyKnockback(knockDir * knockbackForce);
        }

        hasDealtDamage = true;
    }

    private void EndAttack(float time)
    {
        lastAttackTime = time;
        isAttacking = false;
    }
}