using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimator : MonoBehaviour
{
    [Header("Target object with animator")] public GameObject target;
    [Header("Animator trigger name")] public string triggerName;
    private Animator animator;

    void Start()
    {
        animator = target.transform.GetComponentInParent<Animator>();
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            animator.SetTrigger(triggerName);
            enabled = false;
        }
    }

    void OnDrawGizmos() {   
        // Draw a semitransparent red cube at the transforms position
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(transform.position, boxCollider.size);
    }
}
