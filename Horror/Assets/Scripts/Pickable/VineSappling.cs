using FMODUnity;
using UnityEngine;

public class VineSappling : Pickable
{
    public override void interact()
    {
        FMODUnity.RuntimeManager.PlayOneShot(pickUpSound, transform.position);
        transform.position -= new Vector3(0, 10, 0);
    }
}