using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3f;
    private float currentHealth;
    private Animator anim;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; 

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Enemy took damage: " + damage);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            SoundEffectManager.Play("TakeDamage");
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;
            anim.SetTrigger("die");
            Destroy(gameObject, 1.5f); 
        }
    }
}
