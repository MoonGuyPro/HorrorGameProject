using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputAnimator : OutputLogic
{
    public Animator animator;

    protected override void behavior()
    {
        animator = transform.GetComponentInParent<Animator>(); // Yes, this looks silly, but I'm bad at C#
        animator.SetBool("active", active);
    }
}
