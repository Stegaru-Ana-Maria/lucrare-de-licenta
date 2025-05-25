using UnityEngine;

public class AttackState : EnemyState
{
    private float cooldownTimer = 2;
    private bool canDealDamage;

    public AttackState(EnemyFSM enemy) : base(enemy) { }

    public override void EnterState()
    {
        Debug.Log("Enter: AttackState!");
        SoundEffectManager.Play("Sword");
        enemy.anim.SetTrigger("attack");
        cooldownTimer = enemy.attackCooldown;
    }

    public override void UpdateState()
    {
        cooldownTimer -= Time.deltaTime;

        RaycastHit2D hit = Physics2D.Linecast(enemy.enemy.position, enemy.player.position, enemy.obstacleLayer);

        if (hit.collider != null)
        {
            enemy.ChangeState(new PatrolState(enemy));
            return;
        }

        if (!PlayerInAttackRange())
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        FacePlayer();

        if (cooldownTimer <= 0)
        {
            enemy.anim.SetTrigger("attack");
            cooldownTimer = enemy.attackCooldown;
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exit: AttackState!");
    }

    private void FacePlayer()
    {
        if (enemy.player.position.x > enemy.enemy.position.x)
            enemy.enemy.localScale = new Vector3(Mathf.Abs(enemy.enemy.localScale.x), enemy.enemy.localScale.y, enemy.enemy.localScale.z);
        else
            enemy.enemy.localScale = new Vector3(-Mathf.Abs(enemy.enemy.localScale.x), enemy.enemy.localScale.y, enemy.enemy.localScale.z);
    }

    private bool PlayerInAttackRange()
    {
        float distance = Vector2.Distance(enemy.enemy.position, enemy.player.position);
       // float verticalDistance = Mathf.Abs(enemy.enemy.position.y - enemy.player.position.y);

        return distance <= enemy.attackRange;
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
