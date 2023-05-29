using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputMultiAnimator : OutputLogic
{
    [Header("Target objects with animator")] public Animator[] animators;
    [Header("Animator bool name")] public string boolName = "active";

    protected override void Behavior()
    {
        foreach(Animator animator in animators)
        {
            animator.SetBool(boolName, active);
        }
    }
}
