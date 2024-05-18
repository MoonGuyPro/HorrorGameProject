using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class TriggerAmbiance : MonoBehaviour
{
    
    [SerializeField] private EventReference eventReference;
    private EventInstance instance;

    [SerializeField] 
    private bool playAtStart;

    private bool isPlaying = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!isPlaying)
        {
            instance = RuntimeManager.CreateInstance(eventReference);
            instance.start();
            isPlaying = true;
        }
    }

    private void OnDestroy()
    {
        instance.stop(STOP_MODE.IMMEDIATE);
    }
}
