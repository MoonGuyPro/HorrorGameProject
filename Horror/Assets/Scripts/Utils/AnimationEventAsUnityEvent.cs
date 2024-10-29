using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventAsUnityEvent : MonoBehaviour
{
    public UnityEvent[] AnimationEvent;

    void Invoke(int AnimationEventIndex)
    {
        AnimationEvent[AnimationEventIndex].Invoke();
    }
}
