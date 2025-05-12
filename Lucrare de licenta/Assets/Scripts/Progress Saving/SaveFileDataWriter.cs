using System;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "level_progression_data";

    public bool CheckToSeeIfFileExists()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CreateSaveFile(LevelProgressionData progressionData)
    {
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("CREATING SAVE FILE, AT SAVE PATH:" + savePath);

            string dataToStore = JsonUtility.ToJson(progressionData, true);

            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR WHILST TRYING TO SAVE LEVEL PROGRESSION DATA, FILE NOT SAVED " + savePath + "\n" + ex);
        }
    }

    public LevelProgressionData LoadSaveFile()
    {
        LevelProgressionData progressionData = null;

        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {

            try
            {
                string datatoLoad = "";

                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        datatoLoad = reader.ReadToEnd();
                    }
                }

                progressionData = JsonUtility.FromJson<LevelProgressionData>(datatoLoad);
            }
            catch (Exception ex)
            {

            }

        }
        else
        {
            Debug.LogError("THE FILE DOESN'T EXISTS!");
        }

        return progressionData;
    }

    public void UpdateProgresisonFile(LevelProgressionData newProgressionData)
    {
        string updatePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(updatePath))
        {

            try
            {
                string dataToStore = JsonUtility.ToJson(newProgressionData, true);

                File.WriteAllText(updatePath, dataToStore);

                Debug.Log("FILE UPDATED SUCCESSFULLY.");
            }
            catch (Exception ex)
            {
                Debug.LogError("ERROR WHILST TRYING TO UPDATE LEVEL PROGRESSION DATA, FILE NOT UPDATED " + updatePath + "\n" + ex);
            }

        }
        else
        {
            Debug.LogError("THE FILE DOESN'T EXISTS!");
        }
    }
}

