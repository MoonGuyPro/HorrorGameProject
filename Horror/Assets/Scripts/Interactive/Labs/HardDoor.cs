using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class HardDoor : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void OnOpen()
    {
        animator.SetFloat("direction", 1);
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.0f)
            animator.Play("HardDoorOpen", 0 , 0.0f);
        else
            animator.Play("HardDoorOpen");

        // Debug.Log("OnOpen");
        // if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
        // animator.Play("HardDoorOpen");
        // animator.SetBool("Open", true);
        // if (soundEmitter != null)
        //     soundEmitter.Play();
        //                 else
        //     Debug.Log("Kaczka");
    }

    public void OnClose()
    {
        animator.SetFloat("direction", -1);
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            animator.Play("HardDoorOpen", 0 , 1.0f);
        else
            animator.Play("HardDoorOpen");

        // Debug.Log("OnClose");
        // if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
        // animator.Play("HardDoorClose");
        // animator.SetBool("Open", false);
        // if (soundEmitter != null)
        //     soundEmitter.Play();
        //     else
        //     Debug.Log("Kaczka");
    }

}
