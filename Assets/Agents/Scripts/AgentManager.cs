using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    List<Agent> agents;
    List<Vector3> points;
    List<List<Vector3>> pointLists;
    float timer;


    [ContextMenu("Ping Server")]
    public void GetWaypointsFromServer()
    {
        client.Ping(numberOfAgents, pathLength, AcceptWaypoints);
    }

    private void Awake()
    {
        agents = new();
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
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 3)
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
        foreach(Vector3 v in points)
        {
            Debug.Log(v);
        }
    }
}
