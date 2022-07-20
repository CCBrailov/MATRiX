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
    float randInterval;
    float timer = 0;

    void Awake()
    {
        manager = FindObjectOfType<AgentManager>();
        NextInterval();
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
            a.transform.SetPositionAndRotation(transform.position, transform.rotation);
            a.waypoints.Add(transform.position + transform.forward.normalized * 3);
            a.waypoints.Add(transform.position + transform.forward.normalized * 6);
            a.StartNav();
        }
    }
}
