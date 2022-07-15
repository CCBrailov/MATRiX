using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Agent : MonoBehaviour
{
    public GameObject waypointObject;
    public List<Vector3> waypoints;
    public WaypointSystem source;
    public GameObject pointPrefab;
    public List<GameObject> pointViews;

    bool stopTriggered = false;

    int step = 1;
    NavMeshAgent navAgent;
    Animator animator;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        waypoints = new();
        pointViews = new();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        LoadWaypointsFromSource();
        
        if(waypoints.Count > 0)
        {
            navAgent.destination = waypoints[0];
        }
    }

    private void Update()
    {
        animator.SetBool("Moving", navAgent.velocity != new Vector3(0, 0, 0) && !stopTriggered);
        
        if(navAgent.remainingDistance < 0.8 && step == waypoints.Count)
        {
            animator.SetTrigger("Stopping");
            stopTriggered = true;
        }
        
        if(navAgent.velocity.normalized != new Vector3(0, 0, 0))
        {
            transform.rotation = Quaternion.LookRotation(navAgent.velocity.normalized);
        }

        bool arrived = navAgent.remainingDistance < 2;

        if (arrived && step < waypoints.Count)
        {
            navAgent.destination = waypoints[step];
            step++;
        }
    }

    [ContextMenu("Request Waypoints")]
    void LoadWaypointsFromSource()
    {
        foreach(GameObject g in pointViews)
        {
            Destroy(g, 0.1f);
        }
        waypoints = source.GetWaypoints(this);
        foreach (Vector3 pos in waypoints)
        {
            GameObject obj = Instantiate(pointPrefab);
            pointViews.Add(obj);
            obj.transform.position = pos;
        }
        if (waypoints.Count > 0)
        {
            StartNav();
        }
    }

    void StartNav()
    {
        step = 0;
        navAgent.destination = waypoints[0];
    }

    public void NewWaypoints(List<Vector3> points)
    {
        waypoints.Clear();
        waypoints = points;
        foreach (GameObject g in pointViews)
        {
            Destroy(g, 0.1f);
        }
        foreach (Vector3 pos in waypoints)
        {
            GameObject obj = Instantiate(pointPrefab);
            pointViews.Add(obj);
            obj.transform.position = pos;
        }

        //StartNav();
    }

}
