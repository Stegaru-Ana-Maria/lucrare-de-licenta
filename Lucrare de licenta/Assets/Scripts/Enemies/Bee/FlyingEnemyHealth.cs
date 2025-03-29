using UnityEngine;
using System.Collections;

public class FlyingEnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3f;
    private float currentHealth;
    private Animator anim;
    private bool isDead = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Enemy took damage: " + damage);

        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
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