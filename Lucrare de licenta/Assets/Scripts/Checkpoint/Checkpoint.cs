using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private PlayerRespawn respawn;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerRespawn>().UpdateCheckpoint(transform.position);
        }
    }
}
