using UnityEngine;

public class Boss2RoomTrigger : MonoBehaviour
{
    [SerializeField] private Boss2AI bossAI;
    [SerializeField] private AudioClip bossMusic;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            Debug.Log("Player-ul a intrat in camera boss-ului!");
            bossAI.SetPlayerInRoom(true);
            MusicManager.ChangeMusic(bossMusic);
            hasTriggered = true;
        }
    }
}