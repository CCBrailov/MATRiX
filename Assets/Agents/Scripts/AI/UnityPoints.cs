using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityPoints
{
    public UnityPoints()
    {

    }

    public List<Vector3> GetWaypoints()
    {
        List<Vector3> waypoints = new();
        for(int i = 0; i < 4; i++)
        {
            float x = Random.Range(-9, 9);
            float y = 0;
            float z = Random.Range(-9, 9);
            Vector3 wp = new(x, y, z);
            waypoints.Add(wp);
        }
        return waypoints;
    }
}
