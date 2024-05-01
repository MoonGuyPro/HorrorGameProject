using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Oclusion : MonoBehaviour
{
    private Transform player;
    
    [SerializeField]
    EventReference eventReference;
    private EventInstance instance;

    [SerializeField] 
    private string playerName = "NewPlayer";
    
    [SerializeField, Range(0.0f, 1.0f)]
    private float maxOcclusionFilterAmount = 0.1f;
    
    Coroutine occlusionCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        // find the player
        player = GameObject.FindGameObjectWithTag("Player").transform;
        instance = RuntimeManager.CreateInstance(eventReference);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        instance.start();
        
        occlusionCoroutine = StartCoroutine(CheckOcclusion());
    }

    IEnumerator CheckOcclusion()
    {
        // fire a raycast from the audio source to the player
        RaycastHit hit;
        while (true)
        {
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit))
            {
                // if the raycast hits the player, then the audio source is not occluded
                if (hit.transform.gameObject.name != playerName)
                {
                    var result = instance.setParameterByName("Occlusion", maxOcclusionFilterAmount);
                    /Debug.Log(result + "occluded");
                }
                else
                {
                    var result = instance.setParameterByName("Occlusion", 0.0f);
                    Debug.Log(result + "not occluded");
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
