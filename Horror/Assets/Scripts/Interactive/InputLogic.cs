using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for any input object (ex. button, lever)
public abstract class InputLogic : Interactive
{
	public bool singleUse;
	
    [Header("Default state")] public bool active;
    
    // Reference to output object
    protected OutputLogic output;
    public OutputLogic Output
    {
        get { return output; }
        set { output = value; }
    }

    // Input behavior after toggle (ex. animation)
    // Implement in extended class
    protected abstract void Behavior();

    // Interaction toggles state and updates output
    public override bool Interact()
    {
        //print("sdfdssdff");
		if (isActive) {
			active = !active;
			Behavior(); // Call input behavior (implemented in extended class)
			
			// output can be null if there is no output object.
			// for example click to trigger audio.
			if (output != null)
			{
				output.CheckState(); // Check state of output
			}
			
			// Prevent further use if in single use mode
			if (singleUse) {
				isActive = false;
			}
		}
        return true;
    }
}
