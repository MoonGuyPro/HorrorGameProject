using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class GateController : MonoBehaviour
{
    public GameObject leftGate;
    public GameObject rightGate;

    [SerializeField]
    private float animationSeconds = 2f;

    private bool leftButtonTriggered = false;
    private bool rightButtonTriggered = false;

    public UnityEvent onGateOpened;

    public void OnLeftButtonPressed()
    {
        leftButtonTriggered = true;
        TryOpenGate();
    }

    public void OnRightButtonPressed()
    {
        rightButtonTriggered = true;
        TryOpenGate();
    }

    private void TryOpenGate()
    {
        if (leftButtonTriggered && rightButtonTriggered)
        {
            if (leftGate != null && rightGate != null)
            {
                leftGate.transform.DORotate(
                    new Vector3(0, 0, -93), 
                    animationSeconds, 
                    RotateMode.LocalAxisAdd
                );
                rightGate.transform.DORotate(
                    new Vector3(0, 0, 96), 
                    animationSeconds, 
                    RotateMode.LocalAxisAdd
                );
                onGateOpened?.Invoke();
            }
        }
    }
}
