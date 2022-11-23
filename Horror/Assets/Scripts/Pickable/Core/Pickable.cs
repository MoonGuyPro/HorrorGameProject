using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    public string tip;
    [SerializeField] public new string name;
    [SerializeField] public string ingameName;
    // not needed for now i think
    //[SerializeField] public GameObject obj;
    [SerializeField] protected AudioClip[] pickUpSounds;
    protected Transform objTransform;
    protected AudioSource audioSource;

    
    public abstract void interact();
}
