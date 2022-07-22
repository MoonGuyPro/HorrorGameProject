using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputAnimator : OutputLogic
{
    [Header("Target object with animator")] public Animator animator;
    [Header("Animator bool name")] public string boolName;

    protected override void Behavior()
    {
        animator = transform.GetComponentInParent<Animator>(); // Yes, this looks silly, but I'm bad at C#
        animator.SetBool("active", active);
    }
}
