using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IronPython.Hosting;

public class PythonWaypoints : MonoBehaviour
{
    public List<int[]> coords;
    public int numberOfWaypoints;

    private void Awake()
    {
        GeneratePoints();
    }

    public void GeneratePoints()
    {
        var engine = Python.CreateEngine();

        ICollection<string> searchPaths = engine.GetSearchPaths();

        searchPaths.Add(Application.dataPath);
        searchPaths.Add(Application.dataPath + @"/Plugins/Lib/");
        engine.SetSearchPaths(searchPaths);

        dynamic py = engine.ExecuteFile(Application.dataPath + @"/Python/TestFile.py");
        dynamic waypointMaker = py.Points(numberOfWaypoints);

        coords = new();

        IronPython.Runtime.List points = waypointMaker.makePoints();
        foreach (object o in points)
        {
            IronPython.Runtime.List l = (IronPython.Runtime.List)o;
            int[] point = new int[2];
            l.CopyTo(point, 0);
            coords.Add(point);
        }
    }
}
