using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will trigger the appropriate
/// audio dependent on the object itself
/// </summary>

public class Landscape : MonoBehaviour
{

    private Renderer rend;
    private AudioSource audio;


    //NOTE: audio plays on loop, added in the Unity Inspector
    //Gets the components at the start
    void Start()
    {
        rend = GetComponent<Renderer>();
        audio = GetComponent<AudioSource>();
   
    }

    void Update()
    {
        //DO something 
    }

    void OnTriggerEnter(Collider other)
    {
        //Wouldn't need any conditioning, it will only interact with the sphere

        //Waits for 0.35 seconds
        //Not the best way to do this but was the lesser of two evils 
        //Unity is single-threaded and will make the controller laggy 
        System.Threading.Thread.Sleep(350);

        //Changes the objects colour 
        rend.material.color = Color.yellow;

        //Plays the audio
        audio.Play();
        
    }

    void OnTriggerExit(Collider other)
    {
        //Wouldn't need any conditioning, it will only interact with the sphere
        rend.material.color = Color.white;

        //Pauses the audio when the sphere is no longer touching the object
        audio.Pause();
    }
}
