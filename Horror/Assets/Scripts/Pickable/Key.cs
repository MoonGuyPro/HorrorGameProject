using FMODUnity;
using UnityEngine;

public class Key : OldPickable
{
    [SerializeField] private EventReference keyUseSound;
    private void Start()
    {
        objTransform = transform;
    }

    public override void interact()
    {
        objTransform.position -= new Vector3(0,10,0);
        FMODUnity.RuntimeManager.PlayOneShot(pickUpSound, transform.position);
    }

    public void PlaceKey(KeyHole obj)
    {
        objTransform.position = obj.transform.position;
        FMODUnity.RuntimeManager.PlayOneShotAttached(keyUseSound, obj.gameObject);
    }
}
