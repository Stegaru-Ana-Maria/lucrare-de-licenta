using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveProgressManager : MonoBehaviour
{
    public static SaveProgressManager instance;

    public NextLevelTrigger nextLevelTrigger;

    [Header("SAVE GAME")]
    [SerializeField] bool saveGame;

    [Header("SAVE DATA WRITER")]
    private SaveFileDataWriter saveFileDataWriter;

    [Header("CURRENT LEVEL PROGRESSION")]
    public LevelProgressionData currentProgression;

    [Header("SCENE INDEX")]
    public int currentSceneIndex = 0;

    [Header("LEVEL SCENE INDEX")]
    public int testing_scene_index;
    public int prototype_scene_index;
    public int level_1_scene_index;
    public int level_2_scene_index;
    public int level_3_scene_index;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        LoadLevelProgression();
    }

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }
    }

    public void StartLevel(int newSceneIndex)
    {
        StartCoroutine(LoadLevelScene(newSceneIndex));
    }

    private void LoadLevelProgression()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentProgression = new LevelProgressionData();
            saveFileDataWriter.CreateSaveFile(currentProgression);
        }
        else
        {
            currentProgression = saveFileDataWriter.LoadSaveFile();
        }
    }

    public void SaveGame()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        switch (currentSceneIndex)
        {
            case 1:
                currentProgression.level_2_unlocked = true;
                break;
            case 2:
                currentProgression.level_3_unlocked = true;
                break;
            default:
                break;
        }
        saveFileDataWriter.UpdateProgresisonFile(currentProgression);
    }

    public IEnumerator LoadLevelScene(int newSceneIndex)
    {
        currentSceneIndex = newSceneIndex;
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(newSceneIndex);

        while (!loadOperation.isDone)
        {
            yield return null;
        }
    }
    public int GetSceneIndex()
    {
        return currentSceneIndex;
    }
}
