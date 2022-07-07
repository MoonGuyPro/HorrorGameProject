using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive : MonoBehaviour
{
    public string tip;
    public string altTip;
    public abstract bool interact();
}
