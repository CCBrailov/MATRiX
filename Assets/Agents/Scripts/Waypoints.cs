using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waypoints : MonoBehaviour
{

    public List<Transform> waypoints;
    protected int step = 1;

    protected NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if(waypoints.Count > 0)
        {
            agent.destination = waypoints[0].position;
        }
    }

    private void Update()
    {
        bool arrived = agent.remainingDistance < 2;

        if (arrived && step < waypoints.Count)
        {
            agent.destination = waypoints[step].position;
            step++;
        }
    }

}
