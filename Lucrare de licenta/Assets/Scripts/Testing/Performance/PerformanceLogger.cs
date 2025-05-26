using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PerformanceLogger : MonoBehaviour
{
    public static PerformanceLogger Instance;

    public bool enableLogging = true;

    private float timer;
    public float testDuration = 30f;

    private Dictionary<string, List<float>> aiTimes = new Dictionary<string, List<float>>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void LogTime(string aiType, float time)
    {
        if (!enableLogging) return;

        if (!aiTimes.ContainsKey(aiType))
            aiTimes[aiType] = new List<float>();

        aiTimes[aiType].Add(time);
    }

    public void SaveLog(string filename)
    {
        string path = Application.dataPath + "/Experimente/Performanta/" + filename;
        using (StreamWriter writer = new StreamWriter(path))
        {
            foreach (var ai in aiTimes)
            {
                writer.WriteLine(ai.Key + " Times (ms):");
                foreach (float t in ai.Value)
                    writer.WriteLine(t);
                writer.WriteLine();
            }
        }
        Debug.Log("Log salvat: " + path);
    }
}
