using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PerformanceLogger : MonoBehaviour
{
    public static PerformanceLogger Instance;

    [SerializeField] private string logFilename;

    public bool enableLogging = true;

    private float timer;
    public float testDuration = 30f;

    private Dictionary<string, List<float>> aiTimes = new Dictionary<string, List<float>>();
    private List<float> fpsValues = new List<float>();
    private List<float> cpuFrameTimes = new List<float>();

    private int sampleCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Update()
    {
        if (!enableLogging) return;
        if (timer >= testDuration) return;

        timer += Time.deltaTime;

        LogFPS(FPSCounter.Instance.fps);
        LogCPUFrameTime(Time.unscaledDeltaTime * 1000f); 

        sampleCount++;

        if (timer >= testDuration)
        {
            SaveLog(logFilename);
            enabled = false;
        }

    }

    public void StartLogging(float duration, string filename)
    {
        testDuration = duration;
        timer = 0f;
        sampleCount = 0;
        ClearLog();
        enableLogging = true;
        enabled = true;
        logFilename = filename;
    }

    public void LogTime(string aiType, float time)
    {
        if (!enableLogging) return;

        if (!aiTimes.ContainsKey(aiType))
            aiTimes[aiType] = new List<float>();

        aiTimes[aiType].Add(time);
    }

    public void LogFPS(float fps)
    {
        if (!enableLogging) return;
        fpsValues.Add(fps);
    }

    public void LogCPUFrameTime(float frameTime)
    {
        if (!enableLogging) return;
        cpuFrameTimes.Add(frameTime);
    }

    public void SaveLog(string filename)
    {
        string path = Application.dataPath + "/Experiments/Performance/" + filename;
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("Sample,FSM_Time_ms,FSMPathfinding_Time_ms,BT_Time_ms,FPS,CPU_FrameTime_ms");

            int totalSamples = Mathf.Max(
                aiTimes.ContainsKey("FSM") ? aiTimes["FSM"].Count : 0,
                aiTimes.ContainsKey("Pathfinding") ? aiTimes["Pathfinding"].Count : 0,
                aiTimes.ContainsKey("BT") ? aiTimes["BT"].Count : 0,
                fpsValues.Count,
                cpuFrameTimes.Count
            );

            for (int i = 0; i < totalSamples; i++)
            {
                float fsmTime = (aiTimes.ContainsKey("FSM") && i < aiTimes["FSM"].Count) ? aiTimes["FSM"][i] : 0f;
                float fsmPathTime = (aiTimes.ContainsKey("Pathfinding") && i < aiTimes["Pathfinding"].Count) ? aiTimes["Pathfinding"][i] : 0f;
                float btTime = (aiTimes.ContainsKey("BT") && i < aiTimes["BT"].Count) ? aiTimes["BT"][i] : 0f;
                float fps = (i < fpsValues.Count) ? fpsValues[i] : 0f;
                float cpuTime = (i < cpuFrameTimes.Count) ? cpuFrameTimes[i] : 0f;

                writer.WriteLine($"{i + 1},{fsmTime:F4},{fsmPathTime:F4},{btTime:F4},{fps:F2},{cpuTime:F4}");
            }
        }

        Debug.Log("Log salvat: " + path);
    }

    public void ClearLog()
    {
        aiTimes.Clear();
        fpsValues.Clear();
        cpuFrameTimes.Clear();
    }

}
