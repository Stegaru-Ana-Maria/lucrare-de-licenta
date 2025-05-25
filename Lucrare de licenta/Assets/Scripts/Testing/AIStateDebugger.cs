using UnityEngine;
using System.Collections.Generic;

public class AIStateDebugger : MonoBehaviour
{
    public List<GameObject> aiList = new List<GameObject>();

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(1100, 30, 400, 1000));
        GUILayout.Label("<b>AI Debug States:</b>");

        foreach (GameObject ai in aiList)
        {
            if (ai != null)
            {
                string stateInfo = "";

                if (ai.GetComponent<EnemyFSM>())
                {
                    stateInfo = ai.name + " : " + ai.GetComponent<EnemyFSM>().GetCurrentStateName();
                }
                else if (ai.GetComponent<Boss2AI>())
                {
                    stateInfo = ai.name + " : " + ai.GetComponent<Boss2AI>().GetRootNode().GetNodeName();
                }

                GUILayout.Label(stateInfo);
            }
        }

        GUILayout.EndArea();
    }
}
