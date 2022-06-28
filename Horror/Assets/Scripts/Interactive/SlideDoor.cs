using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoor : OutputLogic
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public int changed = 0;

    protected override void behavior()
    {
        animator = transform.GetComponentInParent<Animator>(); // Yes, this looks silly (compared to Lever), but I'm bad at C#
        animator.SetBool("active", active);
        changed ^= 1;
    }
}
