using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class CubeDrone : MonoBehaviour
{
    [SerializeField]
    private EventReference droneEvent;

    private EventInstance instance;

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
        instance = RuntimeManager.CreateInstance(droneEvent);
        RuntimeManager.AttachInstanceToGameObject(instance, gameObject.transform, gameObject.GetComponent<Rigidbody>());

        instance.setParameterByName(paramNamesList[0], Random.Range(minBandpass, maxBandpass));

        instance.setTimelinePosition(Random.Range(0, 2000));
        instance.start();
    }

    private void OnDestroy()
    {
        StopDrone();
    }

    public void StopDrone()
    {
        instance.stop(STOP_MODE.ALLOWFADEOUT);
    }
}