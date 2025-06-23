using UnityEngine;
using UtilityAI; 

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField] private Brain bossBrain;
    [SerializeField] private AudioClip bossMusic;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            Debug.Log("Player-ul a intrat în camera boss-ului!");
            bossBrain.context.SetData("playerInBossRoom", true);
            MusicManager.ChangeMusic(bossMusic);
            hasTriggered = true;
        }
    }
}
