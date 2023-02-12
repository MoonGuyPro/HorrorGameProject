using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using FMODUnity;
using UnityEngine;

// THIS WAS CREATED FOR PURPOSES OF LABIRYNTHS
// where there wasnt a single output logic but a multitude of them.
public class PlayOnInteractive : OutputLogic
{
    [SerializeField] private EventReference eventRef;
    [SerializeField] private List<GameObject> soundsToBeDestroyed = new List<GameObject>();

    protected override void Behavior()
    {
        if (active)
        {
            RuntimeManager.PlayOneShot(eventRef);
            foreach (var go in soundsToBeDestroyed)
            {
                Destroy(go);
            }
        }
    }
}
