using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{
    protected List<Vector3> waypoints;

    public virtual List<Vector3> GetWaypoints(Agent agent)
    {
        LoadWaypoints(agent);
        return waypoints;
    }

    protected virtual void LoadWaypoints(Agent agent)
    {

    }
}
