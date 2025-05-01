using UnityEngine;

public class MagicBook : MonoBehaviour
{
    public static bool hasCollectedBook = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasCollectedBook = true;
            SoundEffectManager.Play("ItemCollected");
            Debug.Log("Player has collected the magic book!");
            Destroy(gameObject);
        }
    }

}
