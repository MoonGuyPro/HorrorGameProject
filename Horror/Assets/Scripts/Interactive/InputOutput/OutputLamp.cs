using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class OutputLamp : OutputLogic
{
    public Animator animator;
    public string boolName = "active";
    public DummyOutput dummyOutput;

    [SerializeField] private EventReference eventRef;

    protected override void Behavior()
    {
        animator.SetBool(boolName, active);
        dummyOutput.ForceToggle();
        RuntimeManager.PlayOneShot(eventRef, transform.position);
    }
}
