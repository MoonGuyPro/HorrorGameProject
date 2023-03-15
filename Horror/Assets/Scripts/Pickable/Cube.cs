using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : Pickable
{
    public override void interact()
    {
        FMODUnity.RuntimeManager.PlayOneShot(pickUpSound, transform.position);
        gameObject.SetActive(false);
    }
}
