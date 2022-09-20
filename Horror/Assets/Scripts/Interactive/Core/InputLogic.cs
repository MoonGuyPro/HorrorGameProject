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
    
    [Header("Sub-Level Finish")] public bool signalLevelFinish;

    // Input behavior after toggle (ex. animation)
    // Implement in extended class
    protected abstract void Behavior();

    // Toggle state and update output
    protected void Toggle()
    {
        active = !active;
        // output can be null if there is no output object.
        // for example click to trigger audio.
        if (output != null)
            output.CheckState(); // Check state of output
        else
           Debug.LogWarning("InputLogic: Changed state but output is null!"); 
    }

    // Interaction by default toggles state, override if needed
    public override bool Interact()
    {
		if (isActive) {
            Toggle();   // Toggle state
			Behavior(); // Call input behavior (implemented in extended class)
			
			// signal finish of a sub level to global variables 
			if (signalLevelFinish)
			{
				GetComponent<SubLevelFinish>().SignalFinish();
			}
			
			// Prevent further use if in single use mode
			if (singleUse) {
				isActive = false;
			}
		}
        return true;
    }
}
