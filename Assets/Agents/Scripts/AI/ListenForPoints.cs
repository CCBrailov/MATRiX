using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class ListenForPoints : WaypointSystem
{
	#region private members 	
	private TcpClient socketConnection;
	private Thread clientReceiveThread;
	private string receivedString = "";
	UnityEvent waypointsReady;
	Agent agent;
    #endregion

    private void Awake()
    {
		waypointsReady = new();
		waypointsReady.AddListener(FeedWaypointsToAgent);
		agent = GetComponent<Agent>();
    }

	void FeedWaypointsToAgent()
    {
		waypoints = new();
		string[] integers = receivedString.Split(".");
		for (int i = 1; i < integers.Length; i += 2)
		{
			int x = int.Parse(integers[i]);
			int y = 0;
			int z = int.Parse(integers[i + 1]);
			Vector3 pos = new(x, y, z);
			waypoints.Add(pos);
		}
		agent.NewWaypoints(waypoints);
    }

    protected override void LoadWaypoints(Agent agent)
    {
		RequestWaypoints();
		waypoints = new();
	}

    // Use this for initialization 	
    void Start()
	{
		ConnectToTcpServer();
	}

	private void ConnectToTcpServer()
	{
		try
		{
			clientReceiveThread = new Thread(new ThreadStart(ListenForData));
			clientReceiveThread.IsBackground = true;
			clientReceiveThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}
  
	private void ListenForData()
	{
		try
		{
			socketConnection = new TcpClient("localhost", 2063);
			Byte[] bytes = new Byte[1024];
			while (true)
			{
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream())
				{
					int length;
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
					{
						var incommingData = new byte[length];
						Array.Copy(bytes, 0, incommingData, 0, length);
						// Convert byte array to string message. 						
						string serverMessage = Encoding.UTF8.GetString(incommingData);
						Debug.Log("Waypoints received as: " + serverMessage);
						waypointsReady.Invoke();
						receivedString = serverMessage;
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}

	private void RequestWaypoints()
	{
		if (socketConnection == null)
		{
			return;
		}
		try
		{
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream();
			if (stream.CanWrite)
			{
				string clientMessage = "3";
				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.UTF8.GetBytes(clientMessage);
				// Write byte array to socketConnection stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
				Debug.Log("Requesting " + clientMessage + " waypoints");
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}
}
