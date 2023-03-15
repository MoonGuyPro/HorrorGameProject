using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputLamp : OutputLogic
{
    public Animator animator;
    public string boolName = "active";
    public DummyOutput dummyOutput;

    protected override void Behavior()
    {
        animator.SetBool(boolName, active);
        dummyOutput.ForceToggle();
    }
}
