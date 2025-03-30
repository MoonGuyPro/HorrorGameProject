using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scannable : MonoBehaviour
{
    public ScannableData Data;
    public UnityEvent OnScanned;
    public bool IsAudio = false;
    [SerializeField, Tooltip("Audio event for talk sounds.")]
    public FMODUnity.EventReference talkEvent;
}
