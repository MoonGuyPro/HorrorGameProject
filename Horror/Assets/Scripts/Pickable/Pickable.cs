using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    public string tip;
    [SerializeField] public new string name;
    [SerializeField] public string ingameName;
    [SerializeField] public GameObject obj;
    protected Transform objTransform;

    public abstract void interact();
}
