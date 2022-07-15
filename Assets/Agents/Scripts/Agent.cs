using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Agent : MonoBehaviour
{
    public GameObject waypointObject;
    public List<Transform> waypoints;
    public PythonWaypoints source;
    public GameObject pointPrefab;

    bool stopTriggered = false;

    int step = 1;
    NavMeshAgent agent;
    Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        waypoints = new();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        GetWaypoints();
        
        if(waypoints.Count > 0)
        {
            agent.destination = waypoints[0].position;
        }

        NavMeshPath path = new();
        agent.CalculatePath(waypoints[0].position, path);
    }

    private void Update()
    {
        animator.SetBool("Moving", agent.velocity != new Vector3(0, 0, 0) && !stopTriggered);
        
        if(agent.remainingDistance < 0.8 && step == waypoints.Count)
        {
            animator.SetTrigger("Stopping");
            stopTriggered = true;
        }
        
        if(agent.velocity.normalized != new Vector3(0, 0, 0))
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }

        bool arrived = agent.remainingDistance < 2;

        if (arrived && step < waypoints.Count)
        {
            agent.destination = waypoints[step].position;
            step++;
        }
    }

    [ContextMenu("New waypoints")]
    void GetWaypoints()
    {
        waypoints.Clear();
        source.GeneratePoints();
        foreach (int[] wp in source.coords)
        {
            GameObject point = Instantiate(pointPrefab);
            point.transform.position = new(wp[0], 0, wp[1]);
            waypoints.Add(point.transform);
            point.transform.SetParent(waypointObject.transform);
        }
    }
}
