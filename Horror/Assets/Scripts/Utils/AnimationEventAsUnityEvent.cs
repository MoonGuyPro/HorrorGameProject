using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventAsUnityEvent : MonoBehaviour
{
    public UnityEvent[] AnimationEvent;

    // Deprecated
    void Invoke(int AnimationEventIndex)
    {
        AnimationEvent[AnimationEventIndex].Invoke();
    }

    void FireEvents(int AnimationEventIndex)
    {
        AnimationEvent[AnimationEventIndex].Invoke();
    }
}
