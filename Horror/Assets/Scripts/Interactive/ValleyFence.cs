using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleyFence : MonoBehaviour
{
    private Animator animator;
    [Tooltip("This int sets how many sublevels need to be finished before this gate opens.")]
    public int gateNumber;

    private void Start()
    {
        print(StaticGlobalVariables.GetGatesOpened());
        animator = GetComponent<Animator>();
        /*  
         * After player returns to the main valley lvl. This should be called to check if the player
         * managed to trigger an increment in the gatesopened variable possibly triggering opening of this gate.
         */
        
        if (StaticGlobalVariables.GetGatesOpened() >= gateNumber)
        {
            animator.SetBool("active", true);
        }
    }
}