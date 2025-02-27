using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GateController : MonoBehaviour
{
    public GameObject leftGate;
    public GameObject rightGate;

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
                leftGate.GetComponent<Animation>().Play();
                rightGate.GetComponent<Animation>().Play();
                onGateOpened?.Invoke();
            }
        }
    }
}
