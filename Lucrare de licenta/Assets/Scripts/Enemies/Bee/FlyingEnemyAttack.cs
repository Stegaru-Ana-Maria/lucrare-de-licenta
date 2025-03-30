using UnityEngine;

public class FlyingEnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockbackForce = 7f;
    private bool hasAttacked = false;

    public void AttackPlayer(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasAttacked)
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                ApplyKnockback(collision.transform);
                hasAttacked = true;
            }
        }
    }

    public void ResetAttack()
    {
        hasAttacked = false; 
    }

    private void ApplyKnockback(Transform playerTransform)
    {
        Rigidbody2D playerRb = playerTransform.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 knockbackDirection = (playerTransform.position - transform.position).normalized;
            Vector2 knockbackVector = new Vector2(knockbackForce * Mathf.Sign(knockbackDirection.x), knockbackForce * 0.5f);

            playerRb.linearVelocity = Vector2.zero;
            playerRb.AddForce(knockbackVector, ForceMode2D.Impulse);
        }
    }
}

