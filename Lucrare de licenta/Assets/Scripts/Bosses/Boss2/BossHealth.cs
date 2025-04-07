using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 10f;
    [SerializeField] private GameObject healEffect;
    private float currentHP;
    private Animator anim;
    private bool isDead = false;

    public delegate void OnHealthChanged(float currentHP, float maxHP);

    private void Awake()
    {
        currentHP = maxHP;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        anim.SetTrigger("hurt");
        Debug.Log("Boss took damage: " + damage);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (!isDead)
        {
            healEffect.SetActive(true);
            currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
            Debug.Log("Boss healed: " + amount);
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

    public float GetHealth() => currentHP;
    public float GetMaxHealth() => maxHP;
}