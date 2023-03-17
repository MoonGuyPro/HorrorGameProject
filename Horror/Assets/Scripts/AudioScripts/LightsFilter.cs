using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class LightsFilter : MonoBehaviour
{
    [SerializeField] EventReference eventRef;
    private EventInstance instance;
    
    bool isFiletered = true;
    private bool isChanging = false;
    private float currentFilterValue = 0.35f;
   
    void Start()
    {
        instance = RuntimeManager.CreateInstance(eventRef);
        instance.start();
        //instance.release();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "NewPlayer")
        {
            if (isFiletered)
            {
                Debug.Log("changing parameter to 1");
                instance.setParameterByName("LightsMusicFilter", 1.0f);
                isFiletered = false;
            }
            else
            {
                Debug.Log("changing parameter to 0.35");
                instance.setParameterByName("LightsMusicFilter", 0.35f);
                isFiletered = true;
            }
        }
        
    }

    private IEnumerator SmoothChangeParameter()
    {
        if (isFiletered)
        {
            float lerpValue = 0.0f;
            while (lerpValue < 1.0f)
            {
                lerpValue += Time.deltaTime;
                currentFilterValue = Mathf.Lerp(currentFilterValue, 1.0f, lerpValue);
                Debug.Log("setting parameter value to: " + currentFilterValue);
                instance.setParameterByName("LightsMusicFilter", currentFilterValue);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            
            isFiletered = false;
        }
        else
        {
            float lerpValue = 0.0f;
            while (lerpValue < 1.0f)
            {
                lerpValue += Time.deltaTime;
                currentFilterValue = Mathf.Lerp(currentFilterValue, 0.35f, lerpValue);
                instance.setParameterByName("LightsMusicFilter", currentFilterValue);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            isFiletered = true;
        }
    }
}
