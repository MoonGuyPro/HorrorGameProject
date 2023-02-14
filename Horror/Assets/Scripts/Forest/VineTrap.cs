using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineTrap : MonoBehaviour
{
    private FPSController fpsController;
    private Animator anim;
    private float playerHeight = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        fpsController = Camera.main.gameObject.GetComponentInParent<FPSController>();
        anim = GetComponentInChildren<Animator>();
    }
    
    // Called by VineTrigger to activate VineTrap
    public void TrapAndTeleport(Vector3 targetPosition)
    {
        // Coroutine is used to yield animations
        IEnumerator coEnumerator = Trap(targetPosition);
        StartCoroutine(coEnumerator);
    }
    
    // Actual VineTrap implementation
    private IEnumerator Trap(Vector3 targetPosition)
    {
        // Stuck player and teleport vines to him
        fpsController.bStuck = true;
        transform.position = fpsController.transform.position + new Vector3(0, -playerHeight, 0);
        
        // Play closing animation
        anim.SetBool("Opened", false);
        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Teleport
        transform.position = targetPosition;
        fpsController.transform.position = targetPosition + new Vector3(0, playerHeight, 0);
        
        // Play opening animation
        anim.SetBool("Opened", true);
        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        
        // Unstuck player
        fpsController.bStuck = false;
    }
}
