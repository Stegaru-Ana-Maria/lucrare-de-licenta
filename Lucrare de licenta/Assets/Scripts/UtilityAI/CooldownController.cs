using System.Collections.Generic;
using UnityEngine;

public class CooldownController : MonoBehaviour
{
    private Dictionary<string, float> cooldowns = new Dictionary<string, float>();

    public void StartCooldown(string key, float time)
    {
        cooldowns[key] = time;
    }

    void Update()
    {
        List<string> keysToRemove = new List<string>();

        foreach (var key in cooldowns.Keys)
        {
            cooldowns[key] -= Time.deltaTime;
            if (cooldowns[key] <= 0)
            {
                keysToRemove.Add(key);
            }
        }

        foreach (var key in keysToRemove)
        {
            cooldowns.Remove(key);
            // Daca ai context accesibil aici, poti face:
            // context.SetData(key, false);
        }
    }

    public bool IsOnCooldown(string key)
    {
        return cooldowns.ContainsKey(key);
    }
}
