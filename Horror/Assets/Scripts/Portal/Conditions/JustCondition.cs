using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustCondition : MonoBehaviour
{
    public bool value;
    public Collider collider;
    public GameObject player;


    private void Awake()
    {
        value = false;
        collider = GetComponent<Collider>();
    }

    public virtual void ReCheckTrigger()
    {}
}
