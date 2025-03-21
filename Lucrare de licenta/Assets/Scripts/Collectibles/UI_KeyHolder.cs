using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_KeyHolder : MonoBehaviour
{
    [SerializeField] private KeyHolder keyHolder;

    private Transform container;
    private Transform keyTemplate;

    private void Awake()
    {
        container = transform.Find("container");
        keyTemplate = container.Find("keyTemplate");
       // keyTemplate.gameObject.SetActive(false);
        if (container == null)
        {
            Debug.LogError("UI_KeyHolder: container not found!");
        }

        if (keyTemplate == null)
        {
            Debug.LogError("UI_KeyHolder: keyTemplate not found!");
        }
        else
        {
            keyTemplate.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        keyHolder.OnKeysChanged += KeyHolder_OnKeysChanged;
        //UpdateVisual();
    }

    private void KeyHolder_OnKeysChanged(object sender, System.EventArgs e)
    {
        Debug.Log("UI Update triggered!");
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        Debug.Log("Updating UI...");

        foreach (Transform child in container)
        {
            if (child == keyTemplate) continue;
            Debug.Log("Destroying key UI element: " + child.name);
            Destroy(child.gameObject);
        }

        List<Key.KeyType> keyList = keyHolder.GetKeyList();
        Debug.Log("Keys in UI: " + keyList.Count);

        if (keyList.Count == 0)
        {
            return;
        }

        for ( int i = 0; i < keyList.Count; i++)
        {
            Key.KeyType keyType = keyList[i];
            Transform keyTransform = Instantiate(keyTemplate, container);
            keyTemplate.gameObject.SetActive(true);
            keyTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(50 * i, 0);

            Image keyImage = keyTransform.Find("image").GetComponent<Image>();
            switch (keyType)
            {
                default:
                case Key.KeyType.Golden:
                    keyImage.color = Color.yellow;
                    break;
                case Key.KeyType.Silver:
                    keyImage.color = Color.grey;
                    break;
            }
        }
    }

}
