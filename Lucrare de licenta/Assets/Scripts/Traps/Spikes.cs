using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float damageInterval = 1f;
    private Coroutine damageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ApplyDamageOverTime(collision.GetComponent<Health>()));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator ApplyDamageOverTime(Health playerHealth)
    {
        while (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(damage);
            yield return new WaitForSeconds(damageInterval);
        }
        damageCoroutine = null;
    }
}
