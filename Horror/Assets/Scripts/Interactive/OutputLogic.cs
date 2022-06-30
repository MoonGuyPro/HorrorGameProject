using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for any output object (ex. door, platform)
public abstract class OutputLogic : MonoBehaviour
{
    public enum ops { AND, OR, XOR };

    [Header("Input condition")]
    public ops op = ops.AND;    // Logical operation
    public bool invert = false; // Invert result

    [Header("Connected inputs")]
    public List<InputLogic> inputs;

    // Hold info about current state (active/not-active)
    protected bool active;

    // Output behavior after checking current state (ex. animation)
    // Implement in extended class
    protected abstract void behavior();

    public void Start()
    {
        connectOutputs();
    }

    // Called by InputLogic.toggle() to update output state
    public void checkState()
    {
        // Check current state using chosen statement
        switch (op)
        {
            case ops.AND:
                active = (and() != invert); // This is logical XOR
                break;
            case ops.OR:
                active = (or() != invert);
                break;
            case ops.XOR:
                active = (xor() != invert);
                break;
        }

        // Call output behavior (implemented in extended class)
        behavior();
    }

    // Call this on Start, to connect all inputs to this output
    protected void connectOutputs()
    {
        // Add reference of output to every input
        foreach (InputLogic input in inputs)
        {
            input.Output = this;
        }

        // Check starting state
        checkState();
    }

    // AND statement
    private bool and()
    {
        // I like this lambda lol
        return inputs.TrueForAll(x => x.active);
    }

    // OR statement
    private bool or()
    {
        foreach (InputLogic input in inputs)
        {
            if (input.active)
            {
                return true;
            }
        }
        return false;
    }

    // XOR statement
    private bool xor()
    {
        int count = 0;
        foreach (InputLogic input in inputs)
        {
            if (input.active)
            {
                count++;
            }
            if (count > 1)
            {
                return false;
            }
        }
        return count == 1;
    }
}
