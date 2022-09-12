using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCube : MonoBehaviour
{
    Animator animator;
    //Value from the slider, and it converts to speed level
    [SerializeField] float speed = 1f;

    void Start()
    {
        //Get the animator, attached to the GameObject you are intending to animate.
        animator = gameObject.GetComponent<Animator>();
        animator.speed = speed;
    }
}