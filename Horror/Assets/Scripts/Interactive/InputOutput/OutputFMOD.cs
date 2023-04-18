using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

public class OutputFMOD : OutputLogic
{
    private StudioEventEmitter emitter;

    new void Start()
    {
        base.Start();
        emitter = GetComponent<StudioEventEmitter>();
    }
    
    protected override void Behavior()
    {
        if (active)
        {
            emitter.Play();
        }
    }
    
}
