using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyPlatform : OutputLogic
{
    [Tooltip("Maximum range at which block reacts to player position")] 
    public float maxRange = 25;
    [Tooltip("At minimal range and lower block is fully hidden")] 
    public float minRange = 5;
    [Tooltip("How much will platform spin when scaling")] 
    public float spinAngle = 180;
    [Tooltip("Enable spin when scaling platform")] 
    public bool bEnableSpin = true;
    [Tooltip("Platform appears only when activated")] 
    public bool bOnlyActivated = false;
    
    public Transform player;

    private float defaultHeight ;
    private float defaultWidth ;
    private float defaultAngle;
    private float currentScale = 1;
    private bool bScaling = true;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        var localScale = transform.localScale;
        defaultHeight = localScale.y;
        defaultWidth = localScale.x;
        defaultAngle = transform.rotation.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bScaling && !bOnlyActivated)
        {
            float distance = (transform.position - player.position).magnitude;
        
            currentScale = 1 - SuperLerp(distance, 0, 1, minRange, maxRange);
            transform.localScale = new Vector3(currentScale * defaultWidth, currentScale * defaultHeight, currentScale * defaultWidth);
        
            if(bEnableSpin)
            {
                float angle = currentScale / 1 * spinAngle;
                transform.localRotation = Quaternion.Euler(Vector3.up * angle);
            }
        }
        else if (!bScaling)
        {
            currentScale = Mathf.Clamp(currentScale + 0.6f * Time.deltaTime, 0f, 1f);
            transform.localScale = new Vector3(currentScale * defaultWidth, currentScale * defaultHeight, currentScale * defaultWidth);
            if(bEnableSpin)
            {
                float angle = currentScale / 1 * spinAngle;
                transform.localRotation = Quaternion.Euler(Vector3.up * angle);
            }
        }
        else
        {
            transform.localScale = new Vector3(0, 0, 0);
            transform.localRotation = new Quaternion();
        }
    }

    float SuperLerp (float value, float from, float to, float from2, float to2) {
        if (value <= from2)
            return from;
        else if (value >= to2)
            return to;
        return (to - from) * ((value - from2) / (to2 - from2)) + from;
    }

    protected override void Behavior()
    {
        bScaling = active;
    }
}
