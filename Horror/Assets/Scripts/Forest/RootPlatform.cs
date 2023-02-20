using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootPlatform : MonoBehaviour
{
    [Tooltip("Maximum range at which root reacts to player position")] 
    public float maxRange = 25;
    [Tooltip("At minimal range root is fully grown")] 
    public float minRange = 15;

    private Transform player;
    private Vector3 defaultPosition;
    private float maxOffset = -6.0f;
    
    void Start()
    {
        defaultPosition = transform.position;
        player = Camera.main.transform;
    }
    
    void FixedUpdate()
    { 
        float distance = (transform.position - player.position).magnitude;
        
        float currentOffset = SuperLerp(distance, 0, maxOffset, minRange, maxRange);
        transform.position = defaultPosition + new Vector3(0, currentOffset, 0);
    }

    float SuperLerp (float value, float from, float to, float from2, float to2) {
        if (value <= from2)
            return from;
        else if (value >= to2)
            return to;
        return (to - from) * ((value - from2) / (to2 - from2)) + from;
    }
}
