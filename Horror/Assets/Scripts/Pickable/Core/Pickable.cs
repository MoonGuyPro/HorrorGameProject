using FMODUnity;
using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    public PickableData Data;
    public void OnPickUp()
    {
        gameObject.SetActive(false);
    }
}
