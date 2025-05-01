using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    public GameObject gameObj;
    private Rigidbody2D rb;
    public Material damagedMaterial;
    private float hurtTimer;
    private float damagedTime = (float)0.1;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hurtTimer = damagedTime;
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            SoundEffectManager.Play("Hurt");
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("grounded");
                anim.SetTrigger("die");
                if (GetComponent<PlayerMovement>() != null)
                {
                    GetComponent<PlayerMovement>().enabled = false;
                }
                dead = true;
            }
        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    public void RestoreHealth()
    {
        currentHealth = startingHealth; 
    }

    private void Die()
    {
        Invoke("Respawn", 1f); 
    }

    public void ResetDeathState()
    {
        dead = false;  
    }

    public void ApplyKnockback(Vector2 force)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; 
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

}
