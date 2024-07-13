using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityPlatform : MonoBehaviour
{
    [Tooltip("Maximum range at which block reacts to player position")] 
    public float maxRange = 25;
    [Tooltip("At minimal range and lower block is fully hidden")] 
    public float minRange = 5;
    [Tooltip("How much will platform spin when scaling")] 
    public float spinAngle = 180;
    [Tooltip("Affects how fast platform spins (0-1)")]
    [Range(0f, 1f)]
    public float spinSpeed = 0.1f;
    [Tooltip("Enable spin when scaling platform")] 
    public bool bEnableSpin = true;

    private Transform player;
    private Vector3 defaultScale;
    private float currentScaleMul = 1;

    // Start is called before the first frame update
    void Start()
    {
        defaultScale = transform.localScale;
        player = Camera.main.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        float distance = (transform.position - player.position).magnitude;
        
        float targetScaleMul = 1 - SuperLerp(distance, 0, 1, minRange, maxRange); 
        currentScaleMul = Mathf.Lerp(currentScaleMul, targetScaleMul, spinSpeed);
        transform.localScale = defaultScale * currentScaleMul;
    
        if(bEnableSpin)
        {
            float angle = currentScaleMul / 1 * spinAngle;
            transform.localRotation = Quaternion.Euler(Vector3.up * angle);
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
