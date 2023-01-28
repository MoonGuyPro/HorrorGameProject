using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

// THIS WAS CREATED FOR PURPOSES OF LABIRYNTHS
// where there wasnt a single output logic but a multitude of them.
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
