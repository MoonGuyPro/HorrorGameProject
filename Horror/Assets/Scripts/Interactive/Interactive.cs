using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Base Interactive class.
    Each interactive object (activated using 'F') should:
        - extend this class
        - be tagged as "Interactive"
*/

public abstract class Interactive : MonoBehaviour
{
    public string tip;
    public string altTip;
    public abstract bool Interact();
}
