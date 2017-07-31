using UnityEngine;
using System.Collections;


/// <summary>
/// Very simple logging behaviour for the JSON data incoming
/// </summary>
public class PSMoveNetworkLogger : MonoBehaviour {

	void OnEnable() {
		PSMoveJSONHandle.OnJSON += LogNetCoords;
	}
	
	void OnDisable() {
		PSMoveJSONHandle.OnJSON -= LogNetCoords;
	}
	
	
	////////////////////////////////////////
	////////////////////////////////////////
	////////////////////////////////////////
	
	// This will be called by the events of the PSMoveJSONHandle
	void LogNetCoords (string JSONString) {
		Debug.Log (JSONString);
	}
}
