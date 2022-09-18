using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleyFence : OutputLogic
{
    private Animator animator;
    
    protected override void Behavior()
    {
        animator = GetComponent<Animator>();
        // Animate on state change
        animator.SetBool("active", active);
    }
}