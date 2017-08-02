using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// JSON stuff
using MiniJSON;

/// <summary>
/// This script will move an object in the Unity scene
/// based on the data coming from the Network location
/// </summary>
/// 

public class PSMoveNetworkControl : MonoBehaviour {

	private Vector3 currentPosition;
    private Renderer rend; 

    void OnEnable() {
		PSMoveJSONHandle.OnJSON += GetNetCoords;
        rend = GetComponent<Renderer>();
	}

	void OnDisable() {
		PSMoveJSONHandle.OnJSON -= GetNetCoords;
	}

	void Update() {
		transform.position = currentPosition;
    }


	////////////////////////////////////////
	////////////////////////////////////////
	////////////////////////////////////////

	// This will be called by the events of the PSMoveJSONHandle
	void GetNetCoords (string JSONString) {

		// Do something with the tracking data
		PSMoveJSONHandle.JSONData data = PSMoveJSONHandle.CreateJSONObject (JSONString);

        // Map the position to our game world
        float mappedXPos = data.xPos / 100;
        float mappedYPos = -(data.yPos / 100);

        //Do stuff from here 
        // Update the position of this object
		currentPosition = new Vector3(mappedXPos, mappedYPos, 0.0f); // The X position is what we're interested in as the camera is positioned to the right of the person in the headset
        Debug.Log("Position: " + currentPosition);
    }
    void OnTriggerEnter(Collider other)
    {
        switch(other.name)
        {
            case "Portrait1":
                rend.material.color = Color.red;
                break;

            case "Portrait2":
                rend.material.color = Color.blue;
                break;

            case "Landscape":
                rend.material.color = Color.yellow;
                break;

            case "Fireplace":
                rend.material.color = Color.green;
                break;

        }
    }

    void OnTriggerExit(Collider other)
    {
        switch (other.name)
        {
            case "Portrait1":
                rend.material.color = Color.white;
                break;

            case "Portrait2":
                rend.material.color = Color.white;
                break;

            case "Landscape":
                rend.material.color = Color.white;
                break;

            case "Fireplace":
                rend.material.color = Color.white;
                break;

        }
    }
   


}
