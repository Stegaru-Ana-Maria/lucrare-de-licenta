using UnityEngine;

public class AttackState : EnemyState
{
    private float cooldownTimer = 0;
    private bool canDealDamage;

    public AttackState(EnemyFSM enemy) : base(enemy) { }

    public override void EnterState()
    {
        Debug.Log("Enemy switched to AttackState!");
        enemy.anim.SetTrigger("attack");
        cooldownTimer = enemy.attackCooldown;
    }

    public override void UpdateState()
    {
        cooldownTimer -= Time.deltaTime;

        if (!PlayerInAttackRange())
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        if (cooldownTimer <= 0)
        {
            enemy.anim.SetTrigger("attack");
            cooldownTimer = enemy.attackCooldown;
        }
    }

    public override void ExitState()
    {
        Debug.Log("Enemy exited AttackState!");
    }

    private bool PlayerInAttackRange()
    {
        return Vector2.Distance(enemy.enemy.position, enemy.player.position) <= enemy.attackRange;
    }

    public void DamagePlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(enemy.enemy.position, enemy.attackRange, enemy.playerLayer);

        if (hit != null)
        {
            Health playerHealth = hit.GetComponent<Health>();
            if (playerHealth != null)
            {
                Debug.Log("Enemy hit the player!");
                playerHealth.TakeDamage(enemy.damage);

                Vector2 knockbackDirection = (hit.transform.position - enemy.enemy.position).normalized;
                playerHealth.ApplyKnockback(knockbackDirection * enemy.knockbackForce);
            }
        }
        else
        {
            Debug.Log("Attack missed. No player in range.");
        }
 
    }
}
