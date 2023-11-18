using FMODUnity;
using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    public PickableData Data;
    public virtual void PickUp()
    {
        if (!Data.pickUpSound.IsNull)
        {
            RuntimeManager.PlayOneShotAttached(Data.pickUpSound, gameObject);
        }
        
        gameObject.SetActive(false);
    }
}
