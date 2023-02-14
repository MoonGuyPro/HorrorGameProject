using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineTrigger : MonoBehaviour
{
    private VineTrap vineTrap;
    public Transform targetLocation;

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
        vineTrap.TrapAndTeleport(targetLocation.position);
    }
    
}
