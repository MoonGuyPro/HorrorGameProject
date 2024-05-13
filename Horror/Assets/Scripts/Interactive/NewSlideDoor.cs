using UnityEngine;
using UnityEngine.Events;

public class NewSlideDoor : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public bool changed = false;
    [HideInInspector] public bool singleUse = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void OnInteraction()
    {
        if(singleUse && changed)
            return;

        animator.SetBool("active", true);
        changed = true;
    }
}
