using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for any output object (ex. door, platform)
public abstract class OutputLogic : MonoBehaviour
{
    public List<InputLogic> inputs;
    protected bool active;

    public void Start()
    {
        // Add reference of output to every input
        foreach(InputLogic input in inputs)
        {
            input.output = this;
        }
        checkState();
    }

    // Output behavior after checking current state (ex. animation)
    // Implement in extended class
    protected abstract void behavior();

    // Called by InputLogic.toggle() to update output state
    // Only AND statement for now
    public void checkState()
    {
        // If any input is inactive, then output is inactive as well
        foreach(InputLogic input in inputs)
        {
            if (!input.active)
            {
                active = false;
                behavior(); // Call output behavior (implemented in extended class)
                return;
            }
        }

        // All outputs are active, so output is active as well
        active = true;
        behavior(); // Call output behavior (implemented in extended class)
    }
}
