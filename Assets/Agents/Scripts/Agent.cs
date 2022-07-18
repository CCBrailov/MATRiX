using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Agent : MonoBehaviour
{
    public List<Vector3> waypoints;

    [SerializeField]
    GameObject model;
    [SerializeField]
    float arrivalDistance;

    bool stopTriggered = false;

    int step = 1;
    AgentManager agentManager;
    UnityPoints pointMaker;
    NavMeshAgent navAgent;
    Animator animator;
    Color gizmoColor;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agentManager = GetComponentInParent<AgentManager>();
        gizmoColor = Random.ColorHSV(0, 1, 1, 1, 1, 1);
        pointMaker = new();
    }

    private void Start()
    {
        waypoints = pointMaker.GetWaypoints();

        if (waypoints.Count > 0)
        {
            navAgent.destination = waypoints[0];
        }
    }

    private void Update()
    {
        #region Editor Gizmo devcode
        if (agentManager.hideAgentModelsOnSelect && (Selection.Contains(this.gameObject) || Selection.Contains(this.transform.parent.gameObject)))
        {
            HideBody();
        }
        else
        {
            ShowBody();
        }
        #endregion

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

        bool arrived = navAgent.remainingDistance <= arrivalDistance;

        if (arrived && step < waypoints.Count)
        {
            navAgent.destination = waypoints[step];
            step++;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Vector3 yOff = new(0, 1, 0);
        Gizmos.DrawSphere(transform.position + yOff, 0.2f);

        
        for (int i = step - 1; i < waypoints.Count - 1; i++)
        {
            Gizmos.DrawLine(transform.position + yOff, waypoints[step - 1] + yOff);
            Gizmos.DrawLine(waypoints[i] + yOff, waypoints[i + 1] + yOff);
        }
    }

    #region Editor Gizmo devcode
    public void HideBody()
    {
        model.SetActive(false);
    }

    public void ShowBody()
    {
        model.SetActive(true);
    }
    #endregion
}