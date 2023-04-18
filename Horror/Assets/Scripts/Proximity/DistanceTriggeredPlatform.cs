using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class DistanceTriggeredPlatform : MonoBehaviour
{
    [Tooltip("Range at which the platform triggers")]
    public float triggerRange;

    [Tooltip("Range at which the platform falls down")]
    public float deTriggerRange;

    private Transform player;
    private Vector3 defaultPosition;

    private bool trigger = false;
    private bool triggerPrevFrameValue = false;
    private bool firstFrame = false;

    [SerializeField, Tooltip("Time for the animation to fully finish")]
    private float animationTime = 1.0f;

    private float interpolator = 0.0f;
    private float interpolationJump = 0.0f;
    
    [SerializeField, Tooltip("Offset (downwards) from the default position at which the platform hides")]
    private float hideOffset = 3.0f;

    [SerializeField] 
    private EventReference upSound;
    [SerializeField]
    private EventReference downSound;

    void Start()
    {
        player = Camera.main.transform;
        defaultPosition = transform.position;
        interpolationJump = 1 / (100 * animationTime);
        transform.position = defaultPosition + Vector3.down * hideOffset;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (transform.position - player.position).magnitude;
        //Debug.Log(distance);

        if (distance <= triggerRange)
        {
            trigger = true;
            if (triggerPrevFrameValue != trigger)
            {
                firstFrame = true;
                interpolator = 0.0f;
                RuntimeManager.PlayOneShotAttached(upSound, gameObject);
            }
        }

        if (distance >= deTriggerRange)
        {
            trigger = false;
            if (triggerPrevFrameValue != trigger)
            {
                firstFrame = true;
                interpolator = 0.0f;
                RuntimeManager.PlayOneShotAttached(downSound, gameObject);
            }
        }
        triggerPrevFrameValue = trigger;
    }

    private void FixedUpdate()
    {
        if (trigger)
        {
            if (firstFrame)
            {
                interpolator = 0.0f;
                firstFrame = false;
            }
            Vector3 lerpPosition = Vector3.Lerp(transform.position, defaultPosition, interpolator);
            interpolator += interpolationJump;
            Clamp(ref interpolator, 0, 1);
            transform.position = lerpPosition;
        }
        else
        {
            if (firstFrame)
            {
                Debug.Log("Fist frame of falling down");
                interpolator = 0.0f;
                firstFrame = false;
            }
            Vector3 lerpPosition = Vector3.Lerp(transform.position, defaultPosition + Vector3.down * hideOffset, interpolator);
            interpolator += interpolationJump;
            Clamp(ref interpolator, 0, 1);
            transform.position = lerpPosition;
        }
    }

    private void Clamp(ref float x, float min, float max)
    {
        if (x < min)
        {
            x = min;
        }
        if (x > max)
        {
            x = max;
        }
    }
}
