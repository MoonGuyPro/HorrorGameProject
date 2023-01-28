using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMODUnity;
using UnityEngine;

public class DoorSound : MonoBehaviour
{
    [SerializeField]
    private EventReference eventRef;
    
    private SlideDoor sd;
    private bool isChanged;

    public string doorSoundParamName;

    public void PlayDoorSound()
    {
        sd = GetComponent<SlideDoor>();
        isChanged = sd.animator.GetBool("active");
        // Debug.Log("playing door sound");
        // RuntimeManager.PlayOneShotAttached(eventRef, gameObject);
        // // based on the isChanged value change fmod "closed/open" parameter
        if (isChanged)
        {
            PlayOneShotWithParam(eventRef, doorSoundParamName, 1);
        }
        else
        {
            PlayOneShotWithParam(eventRef, doorSoundParamName, 0);
        }
        
    }
    
    public void PlayOneShotWithParam(EventReference eventRef, string paramName, float paramValue)
    {
        var instance = RuntimeManager.CreateInstance(eventRef);
        instance.setParameterByName(paramName, paramValue);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        instance.start();
        instance.release();
    }
}
