using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class WeirdLever : InputLogic
{
    [Header("- WeirdLever -")]
    [SerializeField] private Animator animator;
    [SerializeField] private WeirdLever[] otherLevers; 
    public int changed;

    [SerializeField]
    private EventReference eventRef;

    public void Start()
    {
        changed = 0;
    }

    protected override void Behavior()
    {
        // Animate on state change
        //RuntimeManager.PlayOneShotAttached(eventRef, gameObject);
        
        EventInstance inst = RuntimeManager.CreateInstance(eventRef);
        inst.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        inst.setParameterByName("cube_toggle", active ? 0 : 1);
        inst.start();
        inst.release();
        
        animator.SetBool("active", active);
        changed ^= 1;

        foreach(WeirdLever other in otherLevers)
        {
            other.animator.SetBool("active", active);
            other.active = active;
            if(other.outputs != null)
            {
                foreach(OutputLogic output in other.outputs)
                    output.CheckState(); // Force state check of outputs
            }
        }
    }
}
