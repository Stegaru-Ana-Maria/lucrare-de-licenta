using UnityEngine;

public class BossShield : MonoBehaviour
{
    [SerializeField] private float damageToPlayer = 1f;
    [SerializeField] private float damageInterval = 3f;
    [SerializeField] private float pushbackForce = 5f;

    private Health playerHealth;
    private Rigidbody2D playerRb;
    private float damageTimer = 0f;
    private bool playerInside = false;
    private Transform playerTransform;

    private void Update()
    {
        if (playerInside && playerHealth != null)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
            {
                playerHealth.TakeDamage(damageToPlayer);
                Debug.Log("Player damaged by shield (tick)!");

                if (playerRb != null && playerTransform != null)
                {
                    ApplyRadialPushback();
                }

                damageTimer = damageInterval;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerHealth = collision.GetComponent<Health>();
            playerRb = collision.GetComponent<Rigidbody2D>();
            playerTransform = collision.transform;

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
                Debug.Log("Player damaged by shield (instant)!");

                if (playerRb != null && playerTransform != null)
                {
                    ApplyRadialPushback();
                }

                playerInside = true;
                damageTimer = damageInterval;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
            playerHealth = null;
            playerRb = null;
            playerTransform = null;
        }
    }

    private void ApplyRadialPushback()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        playerRb.linearVelocity = Vector2.zero;
        playerRb.AddForce(direction * pushbackForce, ForceMode2D.Impulse);
    }
}
