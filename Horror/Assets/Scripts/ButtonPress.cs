using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void OnInteraction()
    {
        if (animator != null)
        {
            animator.Play("ButtonPress");
        }
    }
}
