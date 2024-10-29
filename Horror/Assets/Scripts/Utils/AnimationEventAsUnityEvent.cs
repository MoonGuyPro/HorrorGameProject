using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventAsUnityEvent : MonoBehaviour
{
    public UnityEvent AnimationEvent;

    void Invoke()
    {
        AnimationEvent.Invoke();
    }
}
