using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class TimedOneShot : AbstractTimedCall
{
    [SerializeField]
    private EventReference sound;
    protected override void OnInterval()
    {
        RuntimeManager.PlayOneShotAttached(sound, gameObject);
    }
}
