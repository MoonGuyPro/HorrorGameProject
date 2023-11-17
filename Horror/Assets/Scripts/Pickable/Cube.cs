using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cube : OldPickable
{
    private CubeDrone cubeDrone;

    private void Start()
    {
        cubeDrone = GetComponent<CubeDrone>();
    }

    public override void interact()
    {
        FMODUnity.RuntimeManager.PlayOneShot(pickUpSound, transform.position);
        if (cubeDrone is not null)
        {
            cubeDrone.StopDrone();
        }
        gameObject.SetActive(false);
    }
}