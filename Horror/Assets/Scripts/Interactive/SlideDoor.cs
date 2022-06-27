using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoor : OutputLogic
{
    private Animator animator;

    protected override void behavior()
    {
        animator = transform.GetComponentInParent<Animator>(); // Yes, this looks silly (compared to Lever), but I'm bad at C#
        animator.SetBool("active", active);
    }
}
