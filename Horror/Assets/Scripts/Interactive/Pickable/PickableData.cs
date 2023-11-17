using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu]
public class PickableData : ScriptableObject
{
    public string DisplayName;
    public string TipText = "[F] Pick up";
    public EventReference pickUpSound;
}
