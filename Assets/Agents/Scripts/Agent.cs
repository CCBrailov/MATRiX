using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Agent : MonoBehaviour
{
    public List<Vector3> waypoints;

    bool stopTriggered = false;

    int step = 1;
    NavMeshAgent navAgent;
    Animator animator;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        UnityPoints pointMaker = new();
        waypoints = pointMaker.GetWaypoints();

        if (waypoints.Count > 0)
        {
            navAgent.destination = waypoints[0];
        }
    }

    private void Update()
    {
        animator.SetBool("Moving", navAgent.velocity != new Vector3(0, 0, 0) && !stopTriggered);

        if (navAgent.remainingDistance < 0.8 && step == waypoints.Count)
        {
            animator.SetTrigger("Stopping");
            stopTriggered = true;
        }

        if (navAgent.velocity.normalized != new Vector3(0, 0, 0))
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
}