using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InputLogic
{
    private Animator animator;
    private AudioSource s;
    public int changed;

    public void Start()
    {
        changed = 0;
        animator = transform.GetComponentInParent<Animator>();
        s = GetComponent<AudioSource>();
    }

    protected override void Behavior()
    {
        // Animate on state change
        animator.SetBool("active", active);
        s.Play();
        changed ^= 1;
    }
}
