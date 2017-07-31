using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Network stuff
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

// JSON stuff
using MiniJSON;

/// <summary>
/// The Unity Networked PSMove Integration API
/// consists of this single C# script.
/// It simply opens a port on the machine
/// then reads the JSON string from the network.
/// It then calls an event to say that new JSON data has arrived
/// </summary>

public class PSMoveJSONHandle : MonoBehaviour {

	// Small structure for storing the data we get from the network
	public class JSONData {
		public int frameID;
		public float radius;
		public float xPos;
		public float yPos;

		public JSONData(int ID, float r, float x, float y) {
			frameID = ID;
			radius = r;
			xPos = x;
			yPos = y;
		}
	}

	public string hostIP;
	public int port;

	private static int MEGABYTE_CACHE = 1024;
	private TcpClient clientConnection;
	private byte[] readBuffer = new byte[MEGABYTE_CACHE]; // 1MB Cache
	private char[] stringSeperators = {'\n'};

	// Event for when new data arrives
	public delegate void JSONAction (string jsonData);
	public static event JSONAction OnJSON;
	
	// Use this for initialization
	void Start () {
		// setup the TCP socket
		try {
			clientConnection = new TcpClient(hostIP, port);
			clientConnection.GetStream().BeginRead(readBuffer, 0, MEGABYTE_CACHE, new AsyncCallback(DoRead), null);
		} 
		catch (Exception ex) {
			print(ex.Message);
		}	
	}

	void Update() {
		// TODO: Nothing really
	}

	void DoRead(IAsyncResult ar)
	{
		int bytesResult;
		try {
			bytesResult = clientConnection.GetStream().EndRead(ar);
			var jsonString = Encoding.ASCII.GetString(readBuffer, 0, bytesResult);

			// TODO: Think of a better way than this; maybe buffering the JSON frames
			// That might reduce jumping
			string[] splitData = jsonString.Split(stringSeperators, StringSplitOptions.RemoveEmptyEntries);
			string json = splitData[0];

			if (OnJSON != null) {
				OnJSON (json); // Call the event
			}

			try {
				clientConnection.GetStream().BeginRead(readBuffer, 0, 1024, new AsyncCallback(DoRead), null); // Repeat...
			} catch(Exception ex) {
				Debug.Log("Serious issue here. Can't make recursive network call");
				Debug.Log(ex.Message);
			}
				
		} catch(Exception ex) {
			// Just try again; shit happens...
			Debug.Log(ex.Message);

			try {
				clientConnection.GetStream().BeginRead(readBuffer, 0, 1024, new AsyncCallback(DoRead), null); // Repeat...
			} catch(Exception ex2) {
				Debug.Log("Serious issue here. Can't make recursive network call after having already failed");
				Debug.Log(ex2.Message);
			}
		}
	}

	public static JSONData CreateJSONObject(string jsonString) {
		var dict = Json.Deserialize (jsonString) as Dictionary<string, object>;
		
		int frameID = (int) Convert.ToSingle (dict ["Frame ID"]);
		Dictionary<string, object> data = ((dict ["Data"]) as Dictionary<string, object>);
		float radius = Convert.ToSingle (data ["radius"]);
		float xPos = Convert.ToSingle (data ["x"]);
		float yPos = Convert.ToSingle (data ["y"]);

		return new JSONData (frameID, radius, xPos, yPos);
	}

	////////////////////////////////////////////
	////////////////////////////////////////////
	////////////////////////////////////////////
	
	void OnApplicationQuit () {
		// Close the TCP socket
		try {
			clientConnection.Close ();
		} catch (Exception ex) {
			Debug.Log (ex.Message);
		}

	}
}
