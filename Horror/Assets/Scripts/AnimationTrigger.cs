using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public GameObject target;
    private Animator animator;

    void Start()
    {
        animator = target.transform.GetComponentInParent<Animator>();
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            animator.SetTrigger("trigger");
        }
    }
}
