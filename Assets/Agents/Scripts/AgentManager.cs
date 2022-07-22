using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentManager : MonoBehaviour
{
    public bool hideAgentModelsOnSelect = true;
    [HideInInspector]
    public bool atCapacity = false;

    int numberOfAgents = 0;
    [SerializeField]
    int maxAgents = 30;
    int agentsAdded = 0;

    [SerializeField]
    public int pathLength = 4;

    [SerializeField]
    GameObject agentPrefab;
    [SerializeField]
    WaypointClient client;

    bool initialized = false;

    List<Agent> agents;
    List<Vector3> points;
    List<List<Vector3>> paths;

    float timer = 0;

    public void ToggleBodies()
    {
        foreach(Agent a in agents)
        {
            a.forceHide = !a.forceHide;
        }
    }

    [ContextMenu("Ping Server")]
    public void GetWaypointsFromServer()
    {
        client.RequestRandomPoints(numberOfAgents, pathLength, AcceptWaypoints);
    }

    private void Awake()
    {
        agents = new();
        paths = new();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        timer += Time.deltaTime;

        atCapacity = numberOfAgents >= maxAgents;

        if (!initialized && client.IsConnected())
        {
            Debug.Log("Initializing");
            GetWaypointsFromServer();
            initialized = true;
        }

        if(timer >= 6)
        {
            GetWaypointsFromServer();
            timer = 0;
        }
    }

    private void Init()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < numberOfAgents; i++)
        {
            Agent a = Instantiate(agentPrefab, transform).GetComponent<Agent>();
            a.name = $"Agent.{i}";
            a.transform.position = new(Random.Range(-9, 9), 0, Random.Range(-9, 9));
            agents.Add(a);
        }
    }

    List<Vector3> DecodeWaypoints(string s)
    {
        List<Vector3> points = new();
        List<string> separated = new(s.Split(","));
        separated.RemoveAt(0);
        for (int i = 0; i < separated.Count - 1; i += 2)
        {
            float x = float.Parse(separated[i]);
            float z = float.Parse(separated[i + 1]);
            points.Add(new(x, 0, z));
        }
        return points;
    }

    public void AcceptWaypoints()
    {
        string s = client.GetWaypointsFromBuffer();
        //Debug.Log($"Received: {s}");
        paths = new();
        points = new();
        points = DecodeWaypoints(s);

        for(int i = 0; i < points.Count; i += pathLength)
        {
            List<Vector3> path = new();
            for(int j = 0; j < pathLength; j++)
            {
                path.Add(points[i + j]);
            }
            paths.Add(path);
        }
        
        //PROBLEM: This function is called when waypoints come from the server
        //The server runs on a separate thread, so this function is called on a separate thread
        //It can change member variables of agents, but cannot start navigating because that function must be called from the same thread as the navAgent
        for(int i = 0; i < paths.Count; i++)
        {
            agents[i].startTrigger = true;
            agents[i].SetPath(paths[i]);
        }
    }

    public void AddAgent(Agent a)
    {
        agents.Add(a);
        numberOfAgents = agents.Count;
        a.name = $"Agent.{agentsAdded}";
        agentsAdded += 1;
    }

    public void RemoveAgent(Agent a)
    {
        agents.Remove(a);
        Destroy(a.gameObject);
        numberOfAgents = agents.Count;
    }
}
