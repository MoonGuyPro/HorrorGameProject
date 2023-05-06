using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supercube : Pickable
{
    public override void interact()
    {
        Debug.Log("Picked.");
        gameObject.SetActive(false);
    }
}
