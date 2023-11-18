using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using FMODUnity;
using UnityEngine;

public class DistanceRotatingPlatform : MonoBehaviour
{
    private enum Angle
    {
        Pitch,
        Yaw,
        Roll
    };
    
    [Tooltip("Range at which the platform triggers")]
    public float triggerRange;

    [Tooltip("Range at which the platform rotates back")]
    public float deTriggerRange;

    public bool isActive = true;
    private Transform player;
    private Quaternion defaultRotation;

    private bool trigger = false;
    private bool triggerPrevFrameValue = false;
    private bool firstFrame = false;

    [SerializeField, Tooltip("Time for the animation to fully finish")]
    private float animationTime = 1.0f;

    private float interpolator = 0.0f;
    private float interpolationJump = 0.0f;
    
    [SerializeField]
    private Angle eulerAngle = Angle.Pitch;
    public float rotationAngle = 0.0f;

    [SerializeField] 
    private EventReference upSound;
    [SerializeField]
    private EventReference downSound;

    [SerializeField] 
    private bool playsSound = true;

    void Start()
    {
        player = Camera.main.transform;
        defaultRotation = transform.rotation;
        interpolationJump = 1 / (100 * animationTime);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (transform.position - player.position).magnitude;
        //Debug.Log(distance);
        if (isActive)
        {
            if (distance <= triggerRange)
            {
                trigger = true;
                if (triggerPrevFrameValue != trigger)
                {
                    firstFrame = true;
                    interpolator = 0.0f;
                    if (playsSound) // this prevents some FMOD warnings that annoyed me eh
                    {
                        RuntimeManager.PlayOneShotAttached(upSound, gameObject);
                    }

                }
            }
            if (distance >= deTriggerRange)
            {
                trigger = false;
                if (triggerPrevFrameValue != trigger)
                {
                    firstFrame = true;
                    interpolator = 0.0f;
                    if (playsSound)
                    {
                        RuntimeManager.PlayOneShotAttached(downSound, gameObject);
                    }
                }
            }
            
        }
        else
        {
            trigger = false;
            if (triggerPrevFrameValue != trigger)
            {
                firstFrame = true;
                interpolator = 0.0f;
                if (playsSound)
                {
                    RuntimeManager.PlayOneShotAttached(downSound, gameObject);
                }
            }
        }
        triggerPrevFrameValue = trigger;

    }

    private void FixedUpdate()
    {
        Vector3 newRotation = new Vector3(0, 0, 0);
        if (trigger)
        {
            if (firstFrame)
            {
                interpolator = 0.0f;
                firstFrame = false;
            }
            switch (eulerAngle)
            {
                case Angle.Pitch:
                    newRotation.x = rotationAngle;
                    break;
                case Angle.Yaw:
                    newRotation.y = rotationAngle ;
                    break;
                case Angle.Roll:
                    newRotation.z = rotationAngle;
                    break;
            }
            Vector3 lerpPosition = Vector3.Lerp(transform.eulerAngles, newRotation, interpolator);
            interpolator += interpolationJump;
            Clamp(ref interpolator, 0, 1);
            transform.eulerAngles = lerpPosition;
        }
        else
        {
            if (firstFrame)
            {
                //Debug.Log("Fist frame of falling down");
                interpolator = 0.0f;
                firstFrame = false;
            }
            Vector3 lerpPosition = Vector3.Lerp(transform.eulerAngles, newRotation, interpolator);
            interpolator += interpolationJump;
            Clamp(ref interpolator, 0, 1);
            transform.eulerAngles = lerpPosition;
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

    public void OnEvent()
    {
        isActive = false;
        Debug.Log("wtf");
    }
}
