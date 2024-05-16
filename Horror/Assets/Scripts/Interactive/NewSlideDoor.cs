using UnityEngine;
using UnityEngine.Assertions;

public class NewSlideDoor : MonoBehaviour
{
    [Tooltip("When set to true, this door can only be activated once.")]
    public bool oneShot = true;

    [Tooltip("Enable to play broken animation and stop regular behavior.")]
    public bool isBroken = false;

    [HideInInspector] public Animator animator;
    [HideInInspector] public bool changed = false;

    private bool toggledOnce = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Assert.IsNotNull(animator);
    }

    void Start()
    {
        if (!isBroken)
            return;

        animator.SetBool("active", false);
        animator.SetBool("broken", true);
    }
    
    public void OnInteraction()
    {
        if (isBroken || (oneShot && toggledOnce))
            return;

        changed = !changed;
        animator.SetBool("active", changed);
        toggledOnce = true;
    }
}
