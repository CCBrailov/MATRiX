using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileWriter : MonoBehaviour
{
    public AgentManager manager;
    public PointConverter converter;

    float timer = 0;
    string path = @"C:\Users\cb3\Documents\SCAN_Plots\data\zara1\test\MATRiX_full.txt";
    string path2 = @"C:\Users\cb3\Documents\SCAN_Plots\data\zara1\test\temp.txt";
    StreamWriter writer;
    float timeStep = 0.0f;

    void Awake()
    {
        File.Delete(path);
        File.Delete(path2);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 0.5)
        {
            timer = 0;
            Debug.Log($"Timestep: {timeStep}");
            WriteLines();
            StreamReader reader = new StreamReader(path);
            string lines = reader.ReadToEnd();
            reader.Close();
            int startLine = lines.IndexOf($"{timeStep - 210}.0\t");
            if(startLine > -1)
            {
                File.Delete(path2);
                StreamWriter writer2 = new(path2, true);
                writer2.Write(lines[startLine..]);
                writer2.Close();
            }
            Debug.Log(lines.IndexOf($"{timeStep - 200}.0\t"));
        }
    }

    void WriteLines()
    {
        writer = new StreamWriter(path, true);
        foreach(Agent a in manager.agents)
        {
            int id = a.id;
            Vector3 position = a.transform.position;
            float x = (converter.WorldPointToScreen(position).x / 500) * 16;
            float y = (converter.WorldPointToScreen(position).y / 460) * 14;
            string line = $"{timeStep}.0\t{id}.0\t{x}\t{y}";
            writer.WriteLine(line);
        }
        writer.Close();
        timeStep += 10;
    }
}
