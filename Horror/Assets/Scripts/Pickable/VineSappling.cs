using FMODUnity;
using UnityEngine;

public class VineSappling : Pickable
{
    public override void interact()
    {
        FMODUnity.RuntimeManager.PlayOneShot(pickUpSound, transform.position);
        gameObject.SetActive(false);
    }
}