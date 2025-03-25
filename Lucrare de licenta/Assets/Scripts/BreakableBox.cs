using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    [SerializeField] private int hitsToBreak = 2; 
    private int currentHits = 0; 

   // private Animator anim;

   /* private void Awake()
    {
        anim = GetComponent<Animator>(); 
    }
   */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow")) 
        {
            currentHits++;

            Debug.Log("Box hit! Hits: " + currentHits);

            if (currentHits >= hitsToBreak)
            {
                BreakBox();
            }
        }
    }

    private void BreakBox()
    {
        Debug.Log("Box destroyed!");
      //  anim.SetTrigger("destroy"); 
        Destroy(gameObject, 0.5f); 
    }
}

