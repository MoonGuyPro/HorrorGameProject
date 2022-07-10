using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactive
{
    public bool opened = false;
    private Animator animator;

    void Start()
    {
        animator = transform.GetComponentInParent<Animator>();
        animator.SetBool("Opened", opened);
    }

    public override bool Interact()
    {
        // This will set the bool the opposite of what it is.
        opened = !opened;

        // This line will set the bool true so it will play the animation.
        animator = transform.GetComponentInParent<Animator>();
        animator.SetBool("Opened", opened);
        return true;
    }
}