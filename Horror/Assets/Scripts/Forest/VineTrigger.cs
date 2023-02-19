using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineTrigger : MonoBehaviour
{
    public enum Mode
    {
        ChangeLocation,
        ChangeLevel
    };
    
    private VineTrap vineTrap;
    public Mode mode = Mode.ChangeLocation;
    public Transform targetLocation;
    public string targetLevel;

    private void Start()
    {
        vineTrap = GameObject.FindObjectOfType<VineTrap>();
        if (vineTrap == null)
        {
            Debug.Log("VineTrap is missing!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mode == Mode.ChangeLocation)
        {
            vineTrap.TrapAndTeleport(targetLocation.position);
        }
        else
        {
            vineTrap.TrapAndChangeLevel(targetLevel);
        }
    }
    
}
