using UnityEngine;
using UnityEngine.Events;

public class NewSlideDoor : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public bool changed = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void OnInteraction()
    {
        animator.SetBool("active", true);
        changed = true;
    }
}
