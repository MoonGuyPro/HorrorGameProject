using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PlayOnInteractive : OutputLogic
{
    [SerializeField] private EventReference eventRef;

    protected override void Behavior()
    {
        if (active)
        {
            RuntimeManager.PlayOneShot(eventRef);
        }
    }
}
