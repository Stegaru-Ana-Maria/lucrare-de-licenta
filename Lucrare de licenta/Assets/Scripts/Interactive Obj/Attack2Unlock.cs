using UnityEngine;

public class Attack2Unlock : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerAttack2 playerAttack = collision.GetComponent<PlayerAttack2>();
            if (playerAttack != null)
            {
                playerAttack.attack2Unlocked = true;
                GameSession.attack2Unlocked = true;
                SoundEffectManager.Play("PowerUp");
                Debug.Log("Attack unlocked!");
                Destroy(gameObject);
            }
        }
    }
}
