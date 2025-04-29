using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class MathRoomController : MonoBehaviour
{
    private int currentRoomNumber = 143; // to be discussed
    private bool isUpsideDown = false;

    private int leftRoomNumber;
    private int rightRoomNumber;

    /*
        leftDown: n = ⌊n / 2⌋ - 1 (odd)
        leftUp: n = 2⌊n / 3⌋ + 2 (even)
        rightDown: n = n + ⌊n / 3⌋ + 3 (odd)
        rightUp: n = n + ⌊n / 2⌋ - 2 (even)
    */

    public void OnRoomReversed()
    {
        isUpsideDown = !isUpsideDown;
    }

    public void OnLeftDoorEntered()
    {
        currentRoomNumber = leftRoomNumber;
        if (isUpsideDown)
        {
            // leftUp: n = 2⌊n / 3⌋ + 2 (even)
        }
        else
        {
            //leftDown: n = ⌊n / 2⌋ - 1 (odd)
        }
    }

    public void OnRightDoorEntered()
    {
        currentRoomNumber = rightRoomNumber;
        if (isUpsideDown)
        {
            //rightUp: n = n + ⌊n / 2⌋ - 2 (even)
        }
        else
        {
            //rightDown: n = n + ⌊n / 3⌋ + 3 (odd)
        }
    }
}
