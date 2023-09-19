using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_size_change_toggle : MonoBehaviour
{

    Vector3 originalScale = new Vector3(1f, 1f, 1f);
    Vector3 activeScale = new Vector3(2f, 2f, 2f);
    

    public void BallSizeUp()
    {
        transform.localScale = activeScale;
    }
    
    public void BallSizeDown()
    {
        transform.localScale = originalScale;
    }
    
}
