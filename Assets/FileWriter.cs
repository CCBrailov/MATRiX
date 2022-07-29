using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class FileWriter : MonoBehaviour
{
    public AgentManager manager;
    public PointConverter converter;

    public bool readyToPredict;

    public int obs_len = 8;
    public int pred_len = 12;

    float timer = 0;
    string path = @"C:\Users\cb3\Documents\SCAN_Plots\data\MATRiX\full.txt";
    string path2 = @"C:\Users\cb3\Documents\SCAN_Plots\data\MATRiX\sample.txt";
    StreamWriter writer;
    float timeStep = 0.0f;
    float timeFactor = 10;
    float sampleSize;

    void Awake()
    {
        File.Delete(path);
        File.Delete(path2);
        sampleSize = obs_len + pred_len + 1;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 0.5)
        {
            timer = 0;
            //Debug.Log($"Timestep: {timeStep}");
            WriteLines();
            StreamReader reader = new StreamReader(path);
            string lines = reader.ReadToEnd();
            reader.Close();
            int startLine = lines.IndexOf($"{timeStep - timeFactor * sampleSize}.0\t");
            if(startLine > -1)
            {
                File.Delete(path2);
                StreamWriter writer2 = new(path2, true);
                writer2.Write(lines[startLine..]);
                writer2.Close();
            }
            //Debug.Log(lines.IndexOf($"{timeStep - 200}.0\t"));
            readyToPredict = 
                (lines.IndexOf($"{timeStep - timeFactor * sampleSize}") > -1)
                && manager.numPredictables >= 2;
            //Debug.Log(readyToPredict);
        }
    }

    void WriteLines()
    {
        writer = new StreamWriter(path, true);
        //foreach(Agent a in manager.agents.Where(agent => agent.inPredictionSpace))
        foreach (Agent a in manager.agents)
        {
            int id = a.id;
            a.timeStepsLogged += 1;
            Vector3 position = a.transform.position;
            float x = (converter.WorldPointToScreen(position).x / converter.cam.pixelWidth) * 16;
            float y = (converter.WorldPointToScreen(position).y / converter.cam.pixelHeight) * 14;
            string line = $"{timeStep}.0\t{id}.0\t{x}\t{y}";
            writer.WriteLine(line);
        }
        writer.Close();
        timeStep += timeFactor;
    }
}
