using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PythonWaypoints : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        
=======
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
>>>>>>> 9168f8059c9a2d0dc467e4951ae15dbec8d4f3c9
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
