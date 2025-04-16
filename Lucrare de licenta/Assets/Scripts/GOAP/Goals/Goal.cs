using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Dictionary<string, bool> goalState = new Dictionary<string, bool>();

    public void SetGoal(string key, bool value)
    {
        goalState[key] = value;
    }

    public Dictionary<string, bool> GetGoalState()
    {
        return new Dictionary<string, bool>(goalState);
    }

    public void Clear()
    {
        goalState.Clear();
    }  
}
