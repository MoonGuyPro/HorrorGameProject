using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for any input object (ex. button, lever)
public abstract class InputLogic : Interactive
{
	[Header("- Input logic -")]
	[Tooltip("Input cannot be toggled again after interaction.")]
	public bool singleUse;
	
	[Tooltip("Start value of input.")]
	public bool active;
    
    // Reference to output object
    [HideInInspector]
    public List<OutputLogic> outputs;

    //[Header("Sub-Level Finish")] public bool signalLevelFinish;

    [Tooltip("If not empty, then input requires an item in player's inventory. By default it doesn't.")]
    public string requiredItem;

    // Input behavior after toggle (ex. animation)
    // Implement in extended class
    protected abstract void Behavior();

    // Toggle state and update output
    protected void Toggle()
    {
        active = !active;
        // output can be null if there is no output object.
        // for example click to trigger audio.
        foreach(OutputLogic output in outputs)
            output.CheckState(); // Check state of output
        if(outputs.Count == 0)
           Debug.LogWarning("InputLogic: Changed state but output is null!"); 
    }

    // Interaction by default toggles state, override if needed
    public override bool Interact(Inventory inv)
    {
	    // If item is needed, then consume it or return failure (if players doesn't have it)
	    if (requiredItem != "")
	    {
		    if (inv.itemExists(requiredItem))
		    {
			    inv.removeItem(requiredItem);
		    }
		    else
		    {
				return false;
		    }
	    }
	    
		if (isActive) {
            Toggle();   // Toggle state
			Behavior(); // Call input behavior (implemented in extended class)
			
			// signal finish of a sub level to global variables 
			/*if (signalLevelFinish)
			{
				GetComponent<SubLevelFinish>().SignalFinish();
			}*/
			
			// Prevent further use if in single use mode
			if (singleUse) {
				isActive = false;
			}
		}
        return true;
    }
}
