using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for any input object (ex. button, lever)
public abstract class InputLogic : MonoBehaviour
{
    [Header("Default state")]
    public bool active;
    [Header("Leave empty")]
    public OutputLogic output;

    // Input behavior after toggle (ex. animation)
    // Implement in extended class
    protected abstract void behavior();

    // Called after interaction, to toggle state and update output
    public void toggle()
    {
        active = !active;
        behavior(); // Call input behavior (implemented in extended class)
        output.checkState(); // Check state of output
    }
}
