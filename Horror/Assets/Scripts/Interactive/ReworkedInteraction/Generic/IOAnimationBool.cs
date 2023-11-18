using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOAnimationBool : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string boolName;
    [SerializeField] bool defaultState;

    private void Start()
    {
       animator.SetBool(boolName, defaultState); 
    }

    public void OnEvent()
    {
        animator.SetBool(boolName, true);
    }

    public void OffEvent()
    {
        animator.SetBool(boolName, false);
    }
}
