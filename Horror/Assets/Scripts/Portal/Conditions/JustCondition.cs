using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustCondition : MonoBehaviour
{
    public bool value;
    public Collider _collider;
    public GameObject player;


    private void Awake()
    {
        value = false;
        _collider = GetComponent<Collider>();
    }

    public virtual void ReCheckTrigger()
    {}
}
