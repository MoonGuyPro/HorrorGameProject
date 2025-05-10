using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorStateEnterExitEvents : StateMachineBehaviour
{
    public UnityEvent StateEnterEvent;
    public UnityEvent StateExitEvent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StateEnterEvent.Invoke();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StateExitEvent.Invoke();
    }
}
