using UnityEngine;

public class Lava : MonoBehaviour
{
    public GameObject player;
    public Transform respawPoint;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.transform.position = respawPoint.position;
        }
    }
}