using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InputLogic
{
    private Animator animator;
    public int changed;

    public void Start()
    {
        changed = 0;
        animator = transform.GetComponentInParent<Animator>();
    }

    protected override void behavior()
    {
        // Animate on state change
        animator.SetBool("active", active);
        changed ^= 1;
    }
}
