using UnityEngine;

public class AnimationParams : MonoBehaviour
{
    Animator animator;
    
    /* Animation speed params */
    [Header("Speed")]
    [SerializeField] bool isRandom;
    [SerializeField] float constSpeed = 1f;
    [SerializeField] float minSpeed = 1f;
    [SerializeField] float maxSpeed = 1f;
    
    
    /* Animation offset params */
    [Header("Offset")]
    // Min value 0
    [SerializeField] float minOffset;
    // Max value 1
    [SerializeField] float maxOffset = 1f;

    void Start()
    {
        // Get the animator, attached to the GameObject you are intending to animate.
        animator = gameObject.GetComponent<Animator>();
        
        /* Set animation speed */
        if (isRandom)
        {
            System.Random random = new System.Random();
            animator.speed = (float) random.NextDouble() * (maxSpeed - minSpeed) + minSpeed;
        }
        else
        {
            animator.speed = constSpeed;
        }
        
        /* Set animation offset */
        GetComponent<Animator>().SetFloat("Offset", Random.Range(minOffset, maxOffset));
    }
}