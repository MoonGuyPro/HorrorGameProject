using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScannableData : ScriptableObject
{
    public string DisplayName;
    [Multiline] public string Description;
    public bool bScanned = false;
}
