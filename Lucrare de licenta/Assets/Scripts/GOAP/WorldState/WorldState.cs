using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    private Dictionary<string, bool> state = new Dictionary<string, bool>();

    public event System.Action<string, bool> OnStateChanged;

    public void SetState(string key, bool value)
    {
        bool changed = !state.ContainsKey(key) || state[key] != value;
       // if (state.ContainsKey(key) && state[key] == value)
         //   return;

        state[key] = value;
        // OnStateChanged?.Invoke(key, value);
        if (changed)
        {
            Debug.Log($"[WorldState] {key} changed to {value} by: {GetCaller()}");
        }
        //Debug.Log($"[WorldState] {key} set to {value}");
    }

    private string GetCaller()
    {
        var stackTrace = new System.Diagnostics.StackTrace();
        if (stackTrace.FrameCount > 2)
            return stackTrace.GetFrame(2).GetMethod().DeclaringType.Name;
        return "Unknown";
    }

    public bool HasState(string key) => state.ContainsKey(key);

    public bool GetState(string key)
    {
        return state.ContainsKey(key) ? state[key] : false;
    }

    public Dictionary<string, bool> GetAllStates()
    {
        return new Dictionary<string, bool>(state);
    }

    public void RemoveState(string key)
    {
        if (state.ContainsKey(key))
            state.Remove(key);
    }

    public void Clear()
    {
        state.Clear();
    }
}
