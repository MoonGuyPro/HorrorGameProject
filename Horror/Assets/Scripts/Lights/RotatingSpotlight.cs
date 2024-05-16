using DG.Tweening;
using UnityEngine;

public class RotatingSpotlight : MonoBehaviour
{
    [Tooltip("Duration of the rotation (in seconds)")]
    public float speed = 2f;    
    public Ease easeType = Ease.Linear;

    void Start()
    {
        transform.DORotate(
            transform.rotation.eulerAngles + (360f * Vector3.up), 
            speed, 
            RotateMode.FastBeyond360
        )
        .SetEase(easeType)
        .SetLoops(-1, LoopType.Restart); // Loop infinitely
    }
}
