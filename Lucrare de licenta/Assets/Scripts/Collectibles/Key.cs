using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private KeyType keyType;

    public enum KeyType
    {
        Golden,
        Silver
    }

    public KeyType GetKeyType()
    {
        return keyType;
    }
    
}
