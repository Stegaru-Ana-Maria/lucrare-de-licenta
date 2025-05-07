using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool playerNear = false;
    private bool isActivated = false;
    private Animator animator;
    public Transform respawnPoint;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E) && !isActivated)
        {
            isActivated = true;
            animator.SetBool("isActivated", true);
            SoundEffectManager.Play("Checkpoint");
            respawnPoint.transform.position = transform.position;
        }

        if (respawnPoint.transform.position != transform.position)
        {
            isActivated = false;
            animator.SetBool("isActivated", false);
        }
    }
}
