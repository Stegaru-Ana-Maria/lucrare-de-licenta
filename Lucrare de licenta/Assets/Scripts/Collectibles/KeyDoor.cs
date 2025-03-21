using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    [SerializeField] private Key.KeyType keyType;
    private bool isOpen = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>(); 
    }

    public Key.KeyType GetKeyType()
    {
        return keyType;
    }
    public void OpenDoor()
    {
        if (!isOpen)
        {
            animator.SetTrigger("open"); 
            isOpen = true;
        }
    }
    
}
