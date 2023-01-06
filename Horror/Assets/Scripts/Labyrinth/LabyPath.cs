using System.Security.Claims;
using UnityEngine;

public class LabyPath : MonoBehaviour
{
    [Tooltip("Maximum range at which block reacts to player position")] 
    public float maxRange = 7;
    [Tooltip("At minimal range and lower block is fully hidden")] 
    public float minRange = 3;
    
    public Transform player;
    public Animator animator;

    private float defaultHeight = 1;
    private float defaultWidth = 1;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float distance = (transform.position - player.position).magnitude;
        if (distance >= maxRange)
        {
            //animator.enabled = true;
            return;
        }
        else
        {
            //animator.enabled = false;
            float newHeight = SuperLerp(distance, 0, 1, minRange, maxRange) * defaultHeight;
            float newWidth = SuperLerp(distance, 0.5f, 1, minRange, maxRange) * defaultWidth;
            transform.localScale = new Vector3(newWidth, newHeight, newWidth);
        }
    }

    float SuperLerp (float value, float from, float to, float from2, float to2) {
        if (value <= from2)
            return from;
        else if (value >= to2)
            return to;
        return (to - from) * ((value - from2) / (to2 - from2)) + from;
    }
}
