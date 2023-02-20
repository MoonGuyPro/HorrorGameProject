using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

public class Footsteps : MonoBehaviour
{
    [Header("Footsteps")]

    [FormerlySerializedAs("walkingSpeed")]
    [Tooltip("Interval between footsteps in seconds. 0.5 = 2 steps per second")]
    [SerializeField] float footstepInterval = 0.7f;
    
    [Tooltip("Interval between running footsteps in seconds. 0.5 = 2 steps per second")]
    [SerializeField] float runStepsInterval = 0.33f;

    private float currentStepsInterval = 0.5f;
    
    [SerializeField, Tooltip("Audio event for footstep sounds.")]
    private FMODUnity.EventReference footstepEvent;

    private bool isPlaying = false;
    private IEnumerator footstepsCoroutine;

    public string footstepsTypeParameterName = "FootstepsType";
    public float footstepsTypeParameterValue = 0f;

    // Start is called before the first frame update
    void Start()
    {
        footstepsCoroutine = playFootsteps();
    }

    public IEnumerator playFootsteps()
    {
        while (true)
        {
            PlayOneShotWithParam();
            isPlaying = true;
            //Debug.Log("current speed" + currentStepsInterval);
            yield return new WaitForSeconds(currentStepsInterval); 
        }

    }
    
    public void StartFootstepsCoroutine()
    {
        if (!isPlaying)
        {
            StartCoroutine(footstepsCoroutine); 
        }
    }

    public void StopFootstepsCoroutine()
    {
        isPlaying = false;
        StopAllCoroutines();
    }

    public void SetRunningSpeed()
    {
        currentStepsInterval = runStepsInterval;
    }

    public void SetWalkingSpeed()
    {
        currentStepsInterval = footstepInterval;
    }

    public void PlayOneShotWithParam()
    {
        var instance = RuntimeManager.CreateInstance(footstepEvent);
        instance.setParameterByName(footstepsTypeParameterName, footstepsTypeParameterValue);
        instance.start();
        instance.release();
    }
}
