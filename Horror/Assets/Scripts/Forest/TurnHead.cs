using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class TurnHead : MonoBehaviour
{

    public Transform target;
    [SerializeField]
    private int speed = 5;

    [SerializeField] 
    private EventReference sound;
    FMOD.Studio.EventInstance soundEvent;
    bool isPlaying = false;

    private void Awake()
    {
        soundEvent = RuntimeManager.CreateInstance(sound);
        soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
    }

    // Update is called once per frame
    void OnTriggerStay(Collider Other)
    {
        if (Other.gameObject.tag == "Player" && target != null)
        {
            if (!isPlaying)
            {
                soundEvent.start();
                isPlaying = true;
            }
            Vector3 relativePos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            Quaternion current = transform.rotation;

            transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime
                * speed);


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlaying)
        {
            soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isPlaying = false;
        }
    }
}
