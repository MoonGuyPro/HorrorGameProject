using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Lever : InputLogic
{
    private Animator animator;
    public int changed;

    [SerializeField]
    private EventReference eventRef;

    public void Start()
    {
        changed = 0;
        animator = transform.GetComponentInParent<Animator>();
    }

    protected override void Behavior()
    {
        // Animate on state change
        RuntimeManager.PlayOneShotAttached(eventRef, gameObject);
        animator.SetBool("active", active);
        changed ^= 1;
    }
}
