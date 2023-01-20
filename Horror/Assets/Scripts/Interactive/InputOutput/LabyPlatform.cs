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
    
    private Transform player;
    private Vector3 defaultScale;
    private float currentScaleMul = 1;
    private bool bScaling = true;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        defaultScale = transform.localScale;
        player = Camera.main.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bScaling && !bOnlyActivated)
        {
            float distance = (transform.position - player.position).magnitude;
        
            currentScaleMul = 1 - SuperLerp(distance, 0, 1, minRange, maxRange);
            transform.localScale = defaultScale * currentScaleMul;
        
            if(bEnableSpin)
            {
                float angle = currentScaleMul / 1 * spinAngle;
                transform.localRotation = Quaternion.Euler(Vector3.up * angle);
            }
        }
        else if (!bScaling)
        {
            currentScaleMul = Mathf.Clamp(currentScaleMul + 0.6f * Time.deltaTime, 0f, 1f);
            transform.localScale = defaultScale * currentScaleMul;
            if(bEnableSpin)
            {
                float angle = currentScaleMul / 1 * spinAngle;
                transform.localRotation = Quaternion.Euler(Vector3.up * angle);
            }
        }
        else
        {
            currentScaleMul = 0.0f;
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
