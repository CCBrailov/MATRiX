using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointConverter : MonoBehaviour
{
    public Camera cam;
    [SerializeField]
    Vector3 point;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mp = Input.mousePosition;
            Vector3 wp = ScreenPointToWorld(mp);
            Debug.Log("Mouse Point: " + mp);
            Debug.Log("World Point: " + wp);
            Debug.Log("Backwards: " + cam.WorldToScreenPoint(wp));
        }
    }

    public Vector3 ScreenPointToWorld(Vector3 screenPoint)
    {
        Ray ray = cam.ScreenPointToRay(screenPoint);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }

    public Vector3 WorldPointToScreen(Vector3 worldPoint)
    {
        return cam.WorldToScreenPoint(worldPoint);
    }
}
