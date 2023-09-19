using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScannableData : ScriptableObject
{
    public string DiplayName;
    [Multiline] public string Description;
}
