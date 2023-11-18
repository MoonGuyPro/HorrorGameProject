using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cube : Pickable
{
    private CubeDrone cubeDrone;

    private void Start()
    {
        cubeDrone = GetComponent<CubeDrone>();
    }

    public override void PickUp()
    {
        base.PickUp();
        if (cubeDrone is not null)
        {
            cubeDrone.StopDrone();
        }
    }
}