using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu]
public class PickableData : ScriptableObject
{
    public string DisplayName;
    public string TipText = "Pick up";
    public EventReference pickUpSound;
}
