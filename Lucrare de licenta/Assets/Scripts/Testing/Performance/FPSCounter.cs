using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public float fps;
    private float deltaTime;
    public static FPSCounter Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
    }
}