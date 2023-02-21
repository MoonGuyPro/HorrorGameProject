using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class TriggerAmbiance : MonoBehaviour
{
    [SerializeField] private EventReference windSound;
    private EventInstance instance;

    [SerializeField] 
    private bool playAtStart;

    private bool isPlaying = false;
    private int triggerCooldown = 0;
    private void Start()
    {
        instance = RuntimeManager.CreateInstance(windSound);
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
                Debug.Log("STOPPING");
                instance.stop(STOP_MODE.ALLOWFADEOUT);
                isPlaying = false;
            }
            else
            {
                Debug.Log("STARTING");
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
