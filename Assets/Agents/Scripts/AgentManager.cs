using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AgentManager : MonoBehaviour
{
    public PointConverter converter;

    public bool hideAgentModelsOnSelect = true;
    [HideInInspector]
    public bool atCapacity = false;
    public bool applySCANPredictions;
    public int numPredictables;
    public float speedFactor = 1;

    int numberOfAgents = 0;
    [SerializeField]
    int maxAgents = 30;
    int agentsAdded = 0;

    [SerializeField]
    public int pathLength = 4;

    public GameObject agentPrefab;
    [SerializeField]
    WaypointClient client;

    //bool initialized = false;

    public List<Agent> agents;
    List<Vector3> points;
    List<List<Vector3>> paths;

    float timer = 0;
    float tick = 3;

    bool SCANTrigger = false;
    string SCANPoints;

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
        if (applySCANPredictions)
        {
            client.RequestSCANPoints(AcceptSCANPoints);
        }
        else
        {
            client.RequestSCANPoints(Placeholder);
        }
    }

    void Placeholder()
    {
        return;
    }
    void AcceptSCANPoints()
    {
        SCANTrigger = true;
    }

    void ProcessSCANPoints()
    {
        List<List<Vector3>> DecodePoints(string s)
        {
            List<List<Vector3>> paths = new();
            // Step 1: Separate each agent's paths
            List<string> step1 = new(s.Split("|"));
            foreach (string pathString in step1)
            {
                List<Vector3> coords = new();
                // Step 2: Separate each set of xy coordinates
                List<string> step2 = new(pathString.Split(","));
                foreach (string pair in step2)
                {
                    // Step 3: 
                    string[] step3 = pair.Split("/");
                    float x = (float.Parse(step3[0]) * 500) / 16;
                    float y = (float.Parse(step3[1]) * 460) / 14;

                    Vector3 xyz = new(x, y, 0);
                    xyz = converter.ScreenPointToWorld(xyz);
                    coords.Add(xyz);
                }
                paths.Add(coords);
            }
            return paths;
        }

        string s = client.GetWaypointsFromBuffer();
        Debug.Log(s);
        paths = DecodePoints(s);

        List<Agent> predictables = new(agents.Where(agent => agent.predictable));

        Debug.Log($"Recieved {paths.Count} trajectories for {predictables.Count} valid agents");

        #region Debug Logging received (non converted) predictions
        string debugString = "";
        List<string> step1 = new(s.Split("|"));
        foreach (string pathString in step1)
        {
            debugString += $"Path {step1.IndexOf(pathString)} = ";
            List<Vector3> coords = new();
            // Step 2: Separate each set of xy coordinates
            List<string> step2 = new(pathString.Split(","));
            foreach (string pair in step2)
            {
                // Step 3: 
                string[] step3 = pair.Split("/");
                float x = float.Parse(step3[0]);
                float y = float.Parse(step3[1]);

                x = ((int)(x * 100)) / 100.0f;
                y = ((int)(y * 100)) / 100.0f;

                debugString += $"({x}/{y})  ";
            }
            debugString += "\n";
        }
        Debug.Log(debugString);
        #endregion

        for (int i = 0; i < predictables.Count; i++)
        {
            predictables[i].SetPath(paths[i]);
            predictables[i].StartNav();
        }
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
        numPredictables = agents.Where(agent => agent.predictable).Count();

        timer += Time.deltaTime;

        atCapacity = numberOfAgents >= maxAgents;

        //if (!initialized && client.IsConnected())
        //{
        //    Debug.Log("Initializing");
        //    GetWaypointsFromServer();
        //    initialized = true;
        //}

        

        if(timer >= tick)
        {
            UnityPoints.GeneratePaths(this);
            GetWaypointsFromServer();
            timer = 0;
        }

        if (SCANTrigger)
        {
            SCANTrigger = false;
            ProcessSCANPoints();
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

    public void AcceptListWaypoints()
    {
        static List<Vector3> DecodeWaypoints(string s)
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

    public void AcceptAgentWaypoints()
    {
        
        Dictionary<Agent, List<Vector3>> DecodeWaypoints(string s)
        {
            Dictionary<Agent, List<Vector3>> trajectories = new();
            
            //Step 1: Separate the agents
            List<string> step1 = new(s.Split("|"));
            foreach(string str in step1)
            {
                //Step 2: Separate the ID from the path
                string[] step2 = str.Split(":");
                int id = int.Parse(step2[0]);

                //Step 3: Separate and convert the path
                string[] step3 = step2[1].Split(",");
                List<Vector3> path = new();
                for(int i = 0; i < step3.Length; i += 2)
                {
                    float x = float.Parse(step3[i]);
                    float y = float.Parse(step3[i + 1]);
                    path.Add(new Vector3(x, 0, y));
                }

                //Step 4: Add entry to dictionary
                trajectories.Add(GetAgent(id), path);
            }
            return trajectories;
        }

        string s = client.GetWaypointsFromBuffer();
        Dictionary<Agent, List<Vector3>> trajectories = DecodeWaypoints(s);
        foreach(KeyValuePair<Agent, List<Vector3>> pair in trajectories)
        {
            pair.Key.SetPath(pair.Value);
            pair.Key.startTrigger = true;
        }
        Debug.Log(s);
    }

    public void AddAgent(Agent a)
    {
        agents.Add(a);
        numberOfAgents = agents.Count;
        a.name = $"Agent.{agentsAdded}";
        a.id = agentsAdded;
        agentsAdded += 1;
    }

    public void RemoveAgent(Agent a)
    {
        agents.Remove(a);
        Destroy(a.gameObject);
        numberOfAgents = agents.Count;
    }

    Agent GetAgent(int id)
    {
        foreach(Agent a in agents)
        {
            if(a.id == id)
            {
                return a;
            }
        }
        return null;
    }
}

