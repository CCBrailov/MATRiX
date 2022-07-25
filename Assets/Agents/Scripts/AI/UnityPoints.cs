using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityPoints
{
    public static void GeneratePaths(AgentManager am)
    {
        foreach (Agent a in am.agents)
        {
            a.waypoints.Clear();
            float angle = Random.Range(-0.2f, 0.2f);
            Quaternion rotation = a.transform.rotation;
            Vector3 forward = a.transform.forward;
            for (int i = 1; i <= am.pathLength; i++)
            {
                a.waypoints.Add(a.transform.position + (a.transform.forward.normalized * i));
            }
            a.StartNav();
        }
    }
}
