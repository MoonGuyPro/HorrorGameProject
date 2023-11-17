using FMODUnity;
using UnityEngine;

public class VineSappling : OldPickable
{
    public override void interact()
    {
        FMODUnity.RuntimeManager.PlayOneShot(pickUpSound, transform.position);
        gameObject.SetActive(false);
    }
}