using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class StaticGlobalVariables
{
    [Tooltip("This variable tells us how many Fence-Gates are opened. Each Gate opens itself if this number is greater than the gate's number")]
    // this variable should be incremented every time a player finishes a sub-level
    static private int GatesOpened = 0;

    static public void IncrementGates()
    {
        GatesOpened++;
    }

    static public int GetGatesOpened()
    {
        return GatesOpened;
    }
}
