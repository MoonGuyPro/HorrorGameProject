using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class IORotatingBridge : MonoBehaviour
{
    [SerializeField] EventReference eventReference;
    [SerializeField] Animator animator;
    [SerializeField] DistanceRotatingPlatform distanceRotatingPlatform;
    private string boolName = "active";

    private void Start()
    {
       //animator.SetBool(boolName, false); 
    }

    public void OnEvent()
    {
        distanceRotatingPlatform.isActive = false;
        //animator.SetBool(boolName, true);
        //RuntimeManager.PlayOneShotAttached(eventReference, gameObject);
    }
}
