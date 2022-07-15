using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityPoints : WaypointSystem
{
    protected override void LoadWaypoints(Agent agent)
    {
        waypoints = new();
        for(int i = 0; i < 4; i++)
        {
            float x = Random.Range(-9, 9);
            float y = 0;
            float z = Random.Range(-9, 9);
            Vector3 wp = new(x, y, z);
            waypoints.Add(wp);
        }
    }
}
