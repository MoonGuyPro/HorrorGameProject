using FMODUnity;
using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    public string tip;
    [SerializeField] public new string name;
    [SerializeField] public string ingameName;
    // not needed for now i think
    //[SerializeField] public GameObject obj;
    [SerializeField] protected EventReference pickUpSound;
    protected Transform objTransform;


    public abstract void interact();
}
