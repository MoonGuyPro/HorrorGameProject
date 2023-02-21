using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using FMODUnity;
using UnityEngine;

// THIS WAS CREATED FOR PURPOSES OF LABIRYNTHS
// where there wasnt a single output logic but a multitude of them.

// this also destroys
public class PlayOnInteractive : OutputLogic
{
    [SerializeField] private EventReference eventRef;
    [SerializeField] private List<GameObject> soundsToBeDestroyed = new List<GameObject>();

    [SerializeField, Tooltip("Is the event reference to be spatialized? (if one doesnt work try the other ;) )")]
    private bool isSpatial = false;

    protected override void Behavior()
    {
        if (active)
        {
            if (isSpatial)
            {
                RuntimeManager.PlayOneShotAttached(eventRef, gameObject);
            }
            else
            {
                RuntimeManager.PlayOneShot(eventRef);
            }
            foreach (var go in soundsToBeDestroyed)
            {
                Destroy(go);
            }
        }
    }
}
