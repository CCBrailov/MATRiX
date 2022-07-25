using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Agent : MonoBehaviour
{
    public List<Vector3> trajectory;
    public List<Vector3> waypoints;
    public bool startTrigger = false;
    public bool forceHide = false;
    public int id;

    [SerializeField]
    GameObject model;
    [SerializeField]
    float arrivalDistance;

    [SerializeReference]
    int step = 1;
    LineRenderer lineRenderer;
    PastTracker pastTracker;
    AgentManager agentManager;
    //UnityPoints pointMaker;
    NavMeshAgent navAgent;
    Animator animator;
    Color gizmoColor;
    float timer = 0;

    float lifetime = 0;
    float lifespan = 60;

    int startMesh;
    int lastFrameMesh;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agentManager = GetComponentInParent<AgentManager>();
        gizmoColor = Random.ColorHSV(0, 1, 1, 1, 1, 1);
        //pointMaker = new();
    }
    private void Start()
    {
        pastTracker = new(this);

        if (waypoints.Count > 0)
        {
            StartNav();
        }
    }
    private void Update()
    {
        #region Editor Gizmos
        if (agentManager.hideAgentModelsOnSelect && (Selection.Contains(this.gameObject) || Selection.Contains(this.transform.parent.gameObject)))
        {
            HideBody();
        }
        else if (forceHide)
        {
            HideBody();
        }
        else
        {
            ShowBody();
        }
        #endregion

        bool markedForRemoval = false;

        #region Entering/Leaving Prediction Space
        NavMeshHit hit;
        navAgent.SamplePathPosition(NavMesh.AllAreas, 0f, out hit);
        int thisFrameMesh = hit.mask;

        if(lastFrameMesh == 1 && thisFrameMesh == 8)
        {
            Debug.Log(name + " has entered Prediction Space");
            //agentManager.GetWaypointsFromServer();
        }

        if(lastFrameMesh == 8 && thisFrameMesh == 1)
        {
            Debug.Log(name + " is leaving Prediction Space");
            markedForRemoval = true;
        }

        lastFrameMesh = thisFrameMesh;
        #endregion

        #region Lifespan tracking
        lifetime += Time.deltaTime;
        if(lifetime >= lifespan)
        {
            markedForRemoval = true;
        }
        #endregion

        #region Tracking past frames
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            pastTracker.Update();
            timer = 0;
        }
        trajectory = pastTracker.trajectory;
        #endregion

        #region Animation
        bool moving = navAgent.velocity != new Vector3(0, 0, 0);
        animator.SetBool("Moving", moving);
        if (moving)
        {
            transform.rotation = Quaternion.LookRotation(navAgent.velocity.normalized);
        }
        #endregion

        #region Waypoint navigation
        // Switching to next waypoint on arrival
        bool arrived = navAgent.remainingDistance <= arrivalDistance;
        if (arrived && step < waypoints.Count)
        {
            navAgent.destination = waypoints[step];
            navAgent.speed = Vector3.Distance(transform.position, waypoints[step]);
            step++;
        }
        else if (arrived && step == waypoints.Count)
        {
            //agentManager.GetWaypointsFromServer();
        }
        #endregion

        if (startTrigger)
        {
            // startTrigger is set by the Python Client when it has given waypoints to the Agent
            StartNav();
            startTrigger = false;
        }

        if (markedForRemoval)
        {
            agentManager.RemoveAgent(this);
        }
    }
    public void SetPath(List<Vector3> path)
    {
        waypoints = path;
    }
    public string Encode()
    {
        //Format: {id},{x}/{y},{x0}/{y0}
        float x = transform.position.x;
        float y = transform.position.z;
        float x0 = trajectory[0].x;
        float y0 = trajectory[0].z;
        return $"{id},{x}/{y},{x0}/{y0}";
    }

    [ContextMenu("Restart Path")]
    public void StartNav()
    {
        step = 1;
        navAgent.destination = waypoints[0];
        navAgent.speed = Vector3.Distance(transform.position, waypoints[0]);
    }

    #region Editor Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Vector3 yOff = new(0, 1, 0);
        Gizmos.DrawSphere(transform.position + yOff, 0.2f);

        List<Vector3> remainingPoints = new();

        if(waypoints.Count > 0)
        {
            remainingPoints = waypoints.GetRange(step - 1, waypoints.Count - step + 1);
            foreach (Vector3 wp in remainingPoints)
            {
                Gizmos.DrawSphere(wp + yOff, 0.1f);
            }

            for(int i = 1; i < remainingPoints.Count; i++)
            {
                Gizmos.DrawLine(remainingPoints[i - 1] + yOff, remainingPoints[i] + yOff);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + yOff, remainingPoints[0] + yOff);

            Gizmos.color = Color.white;
            foreach(Vector3 v in trajectory)
            {
                Gizmos.DrawSphere(v + yOff, 0.1f);
            }
            Gizmos.DrawLine(trajectory[^1] + yOff, transform.position + yOff);
            for(int i = 1; i < trajectory.Count; i++)
            {
                Gizmos.DrawLine(trajectory[i - 1] + yOff, trajectory[i] + yOff);
            }
        }
    }
    public void HideBody()
    {
        model.SetActive(false);
    }
    public void ShowBody()
    {
        model.SetActive(true);
    }
    #endregion
    
    private class PastTracker
    {
        public List<Vector3> trajectory;
        Agent agent;

        public PastTracker(Agent a)
        {
            trajectory = new();
            agent = a;
            for(int i = 0; i < 8; i++)
            {
                trajectory.Add(agent.transform.position);
            }
        }

        public void Update()
        {
            trajectory.RemoveAt(0);
            trajectory.Add(agent.transform.position);
        }
    }
    
}