using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class TriggerAmbiance : MonoBehaviour
{
    
    [SerializeField, FormerlySerializedAs("windSound")] private EventReference eventReference;
    private EventInstance instance;

    [SerializeField] 
    private bool playAtStart;

    private bool isPlaying = false;
    private int triggerCooldown = 0;
    private void Start()
    {
        instance = RuntimeManager.CreateInstance(eventReference);
        //instance.start();
        if (playAtStart)
        {
            instance.start();
            isPlaying = true;
        }
    }

    private void Update()
    {
        if (triggerCooldown > 0)
        {
            triggerCooldown--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerCooldown == 0)
        {
            if (isPlaying)
            {
                instance.stop(STOP_MODE.ALLOWFADEOUT);
                isPlaying = false;
            }
            else
            {
                instance.start();
                isPlaying = true;
            }

            triggerCooldown = 30;
        }
    }

    private void OnDestroy()
    {
        instance.stop(STOP_MODE.IMMEDIATE);
    }
}
