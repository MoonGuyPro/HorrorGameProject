using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SlideDoor : OutputLogic
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public bool changed = false;

    [SerializeField] 
    private UnityEvent onStateChange;
    
    private new void Start()
    {
        base.Start();
        changed = active;
    }
    protected override void Behavior()
    {
        animator = GetComponent<Animator>(); // Yes, this looks silly (compared to Lever), but I'm bad at C#
        animator.SetBool("active", active);
        if (changed != active)
        {
            onStateChange.Invoke();
        }
        changed = active;
    }
}
