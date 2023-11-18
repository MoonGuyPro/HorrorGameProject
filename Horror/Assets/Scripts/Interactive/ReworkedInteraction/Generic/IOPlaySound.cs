using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class IOPlaySound : MonoBehaviour
{
    [SerializeField] EventReference eventReference;

    public void OnEvent()
    {
        RuntimeManager.PlayOneShotAttached(eventReference, gameObject);
    }
}
