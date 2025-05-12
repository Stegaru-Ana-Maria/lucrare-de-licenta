using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{

    private bool hasTriggered = false;
    [SerializeField] int nextLevelScene;

    private void Awake()
    {
        nextLevelScene = SaveProgressManager.instance.currentSceneIndex + 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true;
            SoundEffectManager.Play("CompleteLevel");
            SaveProgressManager.instance.SaveGame();
            SaveProgressManager.instance.StartLevel(nextLevelScene);
        }
    }
}