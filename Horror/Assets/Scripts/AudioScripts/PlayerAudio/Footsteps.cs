using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class Footsteps : MonoBehaviour
{
    [Header("Footsteps")]
    [SerializeField] GameObject player;
    
    [FormerlySerializedAs("walkingSpeed")]
    [Tooltip("Interval between footsteps in seconds. 0.5 = 2 steps per second")]
    [SerializeField] float footstepInterval  = 0.7f;
    PlayerMovement pm;

    public List<AudioSource> footsteps;

    private bool isPlaying;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        pm = player.GetComponent<PlayerMovement>();
        isPlaying = false;
        AudioSource[] temp = player.GetComponents<AudioSource>();
        foreach (AudioSource aS in temp)
        {
            footsteps.Add(aS);
        }        
        // we're removing all other audio sources because they are not footsteps (how clips are handled here will be changed in the future)
        for (int i = footsteps.Count - 1; i > 3; i--)
        {
            footsteps.Remove(footsteps[i]);
        }
        foreach (AudioSource aS in footsteps)
        {
            aS.volume = 0.6f;
        }

        coroutine = playFootsteps(footstepInterval);
    }

    // Update is called once per frame
    void Update()
    {
        // if we're standing on the ground AND the footsteps are not already playing
        // AND we're moving in any direction, play footsteps
        if (pm.grounded && !isPlaying && (pm.rb.velocity.sqrMagnitude > 10))
        {
            StartCoroutine(coroutine);    
        }
        // if we're jumping or are not moving, dont play footsteps
        if (!pm.grounded || (pm.rb.velocity.sqrMagnitude < 10))
        {
            if (isPlaying)
            {
                stopFootsteps();
            } 
        }
    }
    
    private IEnumerator playFootsteps(float interval)
    {
        while (true)
        {
            footsteps[Random.Range(0, 4)].Play();
            isPlaying = true;
            yield return new WaitForSeconds(footstepInterval); 
        }
    }

    private void stopFootsteps()
    {
        isPlaying = false;
        StopCoroutine(coroutine);
    }
}
