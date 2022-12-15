using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class Footsteps : MonoBehaviour
{
    [Header("Footsteps")]

    [FormerlySerializedAs("walkingSpeed")]
    [Tooltip("Interval between footsteps in seconds. 0.5 = 2 steps per second")]
    [SerializeField] float footstepInterval = 0.7f;
    
    [SerializeField, Tooltip("Audio event for footstep sounds.")]
    private FMODUnity.EventReference footstepEvent;

    private bool isPlaying = false;
    private IEnumerator footstepsCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        footstepsCoroutine = playFootsteps();
    }

    public void StartFootstepsCoroutine()
    {
        if (!isPlaying)
        {
            StartCoroutine(footstepsCoroutine); 
        }
    }
    
    public IEnumerator playFootsteps()
    {
        while (true)
        {
            FMODUnity.RuntimeManager.PlayOneShot(footstepEvent);
            isPlaying = true;
            yield return new WaitForSeconds(footstepInterval); 
        }

    }

    public void StopFootstepsCoroutine()
    {
        isPlaying = false;
        StopCoroutine(footstepsCoroutine);
    }
}
