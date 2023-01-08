using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class CubeDrone : MonoBehaviour
{
    [SerializeField] 
    private EventReference droneEvent;
    
    List<string> paramNamesList = new List<string>();
    List<float> paramValuesList = new List<float>();
    
    public float minBandpass = 0f;
    public float maxBandpass = 1f;
    
    void Start()
    {
        paramNamesList.Add("CubeBandpass");
        PlayOneShotAttachedWithParams();
    }

    void PlayOneShotAttachedWithParams()
    {
        var instance = RuntimeManager.CreateInstance(droneEvent);
        RuntimeManager.AttachInstanceToGameObject(instance, gameObject.transform, gameObject.GetComponent<Rigidbody>());
        
        instance.setParameterByName(paramNamesList[0], Random.Range(minBandpass, maxBandpass));

        instance.setTimelinePosition(Random.Range(0, 2000));
        instance.start();
        instance.release();
    }
}
