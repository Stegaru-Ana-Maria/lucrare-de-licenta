using UnityEngine;

public class KeyFloat : MonoBehaviour
{
    public float floatSpeed = 1f; 
    public float floatAmplitude = 0.2f; 

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position; 
    }

    private void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

}
