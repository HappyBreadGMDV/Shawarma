using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Splines;

public class Drone : MonoBehaviour
{
    public DroneBox BoxScript;
    public PlayableDirector playableDirector;

    //private void Awake()
    //{
        
    //}

    private void Awake()
    {
        playableDirector.Stop();
    }

    public void StartDrone(GameObject[] Objects)
    {
        BoxScript.Objects = Objects;
        playableDirector.Resume();
        playableDirector.Play();
    }
}
