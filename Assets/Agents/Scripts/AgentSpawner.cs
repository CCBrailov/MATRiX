using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    [SerializeField]
    AgentManager manager;
    [SerializeField]
    GameObject agentPrefab;
    [SerializeField]
    float interval = 10;
    [SerializeField]
    bool spawnImmediately;
    float randInterval;
    float timer = 0;

    void Awake()
    {
        manager = FindObjectOfType<AgentManager>();
        agentPrefab = manager.agentPrefab;
        NextInterval();
    }

    void Start()
    {
        if (spawnImmediately)
        {
            SpawnAgent();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= randInterval)
        {
            SpawnAgent();
            timer = 0;
            NextInterval();
        }
    }
    void NextInterval()
    {
        float min = 0.85f * interval;
        float max = 1.15f * interval;
        randInterval = Random.Range(min, max);
    }
    [ContextMenu("Spawn Agent")]
    void SpawnAgent()
    {
        if (!manager.atCapacity)
        {
            Agent a = Instantiate(agentPrefab, manager.transform).GetComponent<Agent>();
            manager.AddAgent(a);
            Vector3 pos = transform.position;
            pos = new(pos.x + Random.Range(-0.8f, 0.8f), pos.y, pos.z + Random.Range(-0.8f, 0.8f));
            a.transform.SetPositionAndRotation(pos, transform.rotation);
            for(int i = 1; i <= manager.pathLength; i++)
            {
                a.waypoints.Add(a.transform.position + (transform.forward.normalized * i));
            }
            a.StartNav();
        }
    }
}
