using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 currentCheckpoint;
    private Health playerHealth;
    private Animator anim;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;

    private void Awake()
    {
        currentCheckpoint = transform.position;
        playerHealth = GetComponent<Health>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateCheckpoint(Vector2 newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    public void Respawn()
    {
        transform.position = currentCheckpoint;
        playerHealth.RestoreHealth();
        playerHealth.ResetDeathState();

        if (anim != null)
        {
            anim.ResetTrigger("die");
            anim.Play("Idle");
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
