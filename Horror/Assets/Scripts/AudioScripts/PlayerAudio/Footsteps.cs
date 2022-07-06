using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [Header("Footsteps")]
    [SerializeField] GameObject player;
    [SerializeField] int walkingSpeed;
    PlayerMovement pm;

    public List<AudioSource> footsteps;

    [HideInInspector] private bool isPlaying;

    CancellationTokenSource cts;
    CancellationToken ct;
    private Task task;
      
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
        for (int i = footsteps.Count - 1; i > 3; i--)
        {
            footsteps.Remove(footsteps[i]);
        }
        foreach (AudioSource aS in footsteps)
        {
            aS.volume = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if we're standing on the ground AND the footsteps are not already playing
        // AND we're moving in any direction, play footsteps
        if (pm.grounded && !isPlaying && (pm.rb.velocity.sqrMagnitude > 10))
        {
            startFootsteps();    
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
   
    private async Task playFootsteps(int interval, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested && isPlaying)
        {
            //print("playing . . .");
            footsteps[Random.Range(0, 4)].Play();
            await Task.Delay(interval, cancellationToken);

            if (!isPlaying)
            {
                break;
            }
        }
    }
    
    private void startFootsteps()
    {
        cts = new CancellationTokenSource();
        ct = cts.Token;
        isPlaying = true;
        task = playFootsteps(walkingSpeed, ct);
        //print("footsteps started");
    }
    
    private void stopFootsteps()
    {
        isPlaying = false;
        cts.Cancel();
        //task.Dispose();
        //print("footsteps stopped");
    }
}
