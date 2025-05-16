using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private float healAmount = 1f;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();

            if (playerHealth != null)
            {
                if (playerHealth.currentHealth < playerHealth.StartingHealth)
                {
                    playerHealth.AddHealth(healAmount);
                    SoundEffectManager.Play("PowerUp");
                    Debug.Log("Picked up heart, healed for " + healAmount);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Health is full - can't pick up heart.");
                }
            }
        }
    }
}
