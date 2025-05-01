using UnityEngine;

public class Attack2PowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundEffectManager.Play("PowerUp");
            collision.GetComponent<PlayerAttack2>().ActivateArrowPowerUp();
            Destroy(gameObject); 
        }
    }
}

