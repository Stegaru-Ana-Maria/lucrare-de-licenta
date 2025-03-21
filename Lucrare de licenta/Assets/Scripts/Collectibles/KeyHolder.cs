using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    public event EventHandler OnKeysChanged;
    private List<Key.KeyType> keyList;

    private void Awake()
    {
        keyList = new List<Key.KeyType>();
    }

    public List<Key.KeyType> GetKeyList()
    {
        return keyList;
    }

    public void AddKey(Key.KeyType keyType)
    {
        Debug.Log("Added " + keyType + " Key");
        keyList.Add(keyType);
        OnKeysChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveKey(Key.KeyType keyType)
    {
        if (keyList.Contains(keyType))
        {
            keyList.Remove(keyType);
            Debug.Log("Removed key: " + keyType);
            Debug.Log("Keys remaining: " + keyList.Count);

            OnKeysChanged?.Invoke(this, EventArgs.Empty);
            Debug.Log("OnKeysChanged event triggered!");
        }
        else
        {
            Debug.LogWarning("Tried to remove key that doesn't exist: " + keyType);
        }
    }

    public bool ContainsKey(Key.KeyType keyType)
    {
        return keyList.Contains(keyType);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Key key = collider.GetComponent<Key>();
        if (key !=null)
        {
            AddKey(key.GetKeyType());
            Destroy(key.gameObject);
        }

        KeyDoor keyDoor = collider.GetComponent<KeyDoor>();
        if (keyDoor != null && ContainsKey(keyDoor.GetKeyType()))
        {
            Debug.Log("Opening door...");
            RemoveKey(keyDoor.GetKeyType());
            keyDoor.OpenDoor();
            Debug.Log("Remaining keys after opening door: " + keyList.Count);
        }

    }

}
