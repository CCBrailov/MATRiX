using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine.Events;
using System.Diagnostics;

public class WaypointClient : MonoBehaviour
{
    [Tooltip("Static Camera used for casting Prediction coordinates to World coordinates")]
    public PointConverter converter;

    TcpClient socket;
    NetworkStream stream;
    byte[] receiveBuffer;
    UnityEvent received;
    string waypointsBuffer = "";
    Process serverProcess;

    private void Awake()
    {
        String path = @"C:\users\cb3\Documents\MATRiX-Python\TestServer.py";
        serverProcess = new();
        serverProcess.StartInfo.FileName = "python";
        serverProcess.StartInfo.Arguments = path;
        serverProcess.Start();

        received = new();

        socket = new TcpClient
        {
            ReceiveBufferSize = 1024,
            SendBufferSize = 1024
        };

        socket.BeginConnect(IPAddress.Parse("127.0.0.1"), 2063, ConnectCallback, socket);
    }

    private void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("Shutting down server");
        serverProcess.Kill();
    }

    
    public void RequestAgentPoints(List<Agent> agents, UnityAction call)
    {
        if (socket.Connected)
        {
            received.AddListener(call);
            string aString = "";
            foreach(Agent a in agents)
            {
                
            }
            stream.Write(Encoding.UTF8.GetBytes(aString));
        }
        else
        {
            UnityEngine.Debug.Log("Server not yet connected");
        }
    }

    public void RequestRandomPoints(int numAgents, int pathLength, UnityAction call)
    {
        if (socket.Connected)
        {
            received.AddListener(call);
            string nStr = "" + (numAgents * pathLength);
            stream.Write(Encoding.UTF8.GetBytes(nStr));
        }
        else
        {
            UnityEngine.Debug.Log("Server not yet connected");
        }
    }

    void ConnectCallback(IAsyncResult asyncResult)
    {
        socket.EndConnect(asyncResult);
        if (!socket.Connected) { return; }
        stream = socket.GetStream();
        receiveBuffer = new byte[1024];
        stream.BeginRead(receiveBuffer, 0, 1024, ReceiveCallback, null);
    }

    void ReceiveCallback(IAsyncResult asyncResult)
    {
        try
        {
            int byteLength = stream.EndRead(asyncResult);
            if(byteLength <= 0)
            {
                return;
            }
            byte[] data = new byte[byteLength];
            Array.Copy(receiveBuffer, data, byteLength);
            waypointsBuffer = Encoding.UTF8.GetString(data);
            UnityEngine.Debug.Log("Received waypoints from server");
            received.Invoke();
            received.RemoveAllListeners();
            stream.BeginRead(receiveBuffer, 0, 1024, ReceiveCallback, null);
        }
        catch(Exception e)
        {
            UnityEngine.Debug.Log($"Error receiving TCP data: {e.Message}");
        }
    }

    public string GetWaypointsFromBuffer()
    {
        string returnStr = waypointsBuffer;
        return returnStr;
    }

    public bool IsConnected()
    {
        return socket.Connected;
    }
}
