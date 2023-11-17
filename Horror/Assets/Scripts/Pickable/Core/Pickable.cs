using FMODUnity;
using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    public PickableData Data;
    public void OnPickUp()
    {
        if (!Data.pickUpSound.IsNull)
        {
            RuntimeManager.PlayOneShotAttached(Data.pickUpSound, gameObject);
        }
        
        gameObject.SetActive(false);
    }
}
