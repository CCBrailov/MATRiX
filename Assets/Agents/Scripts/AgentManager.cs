using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentManager : MonoBehaviour
{
    public bool hideAgentModelsOnSelect = true;

    [SerializeField]
    int numberOfAgents = 1;
    [SerializeField]
    int pathLength = 4;
    [SerializeField]
    GameObject agentPrefab;
    [SerializeField]
    WaypointClient client;

    UnityEvent pathProvided;
    List<Agent> agents;
    List<Vector3> points;
    List<List<Vector3>> paths;
    float timer;


    [ContextMenu("Ping Server")]
    public void GetWaypointsFromServer()
    {
        client.Ping(numberOfAgents, pathLength, AcceptWaypoints);
    }

    private void Awake()
    {
        agents = new();
        paths = new();
        pathProvided = new();
        timer = 0;
    }

    void Start()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < numberOfAgents; i++)
        {
            Agent a = Instantiate(agentPrefab, transform).GetComponent<Agent>();
            a.name = $"Agent.{i}";
            a.transform.position = new(Random.Range(-9, 9), 0, Random.Range(-9, 9));
            agents.Add(a);
        }

        GetWaypointsFromServer();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 10)
        {
            GetWaypointsFromServer();
            timer = 0;
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
        Debug.Log($"Received: {s}");
        points = new();
        points = DecodeWaypoints(s);
        paths = new();
        for(int i = 0; i < points.Count; i += pathLength)
        {
            List<Vector3> path = new();
            for(int j = 0; j < pathLength; j++)
            {
                path.Add(points[i + j]);
            }
            paths.Add(path);
        }
        foreach(List<Vector3> p in paths)
        {
            string debugStr = "";
            foreach(Vector3 v in p)
            {
                debugStr += v.ToString() + " / ";
            }
            Debug.Log(debugStr);
        }
        for(int i = 0; i < paths.Count; i++)
        {
            pathProvided.AddListener(agents[i].StartNav);
            agents[i].SetPath(paths[i]);
            pathProvided.Invoke();
            pathProvided.RemoveAllListeners();
        }
    }
}
