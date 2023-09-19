using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_size_change : MonoBehaviour
{
    private enum BallSize
    {
        Small,
        Medium,
        Large
    }
    
    private BallSize ballSize;
    
    private void Start()
    {
        ballSize = BallSize.Small;
    }

    public void ChangeBallSize()
    {
        switch (ballSize)
        {
            case BallSize.Small:
                transform.localScale = new Vector3(2, 2, 2);
                ballSize = BallSize.Medium;
                break;
            case BallSize.Medium:
                transform.localScale = new Vector3(3, 3, 3);
                ballSize = BallSize.Large;
                break;
            case BallSize.Large:
                transform.localScale = new Vector3(1, 1, 1);
                ballSize = BallSize.Small;
                break;
        }
    }
    
}
