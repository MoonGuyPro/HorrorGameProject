using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubLevelFinish : MonoBehaviour
{
    // increment global gate variable to signal finishing of a sub level
    public void SignalFinish()
    {
        StaticGlobalVariables.IncrementGates();
        print(StaticGlobalVariables.GetGatesOpened());
    }
}
