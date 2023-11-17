using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class InteractionInput : MonoBehaviour
{
    [Header("Visual Tips")]
    [SerializeField] private string tip;
    [SerializeField] private string altTip;
    [Header("Interaction Properties")]
    [SerializeField] private bool singleUse = false;
    private bool canInteract = true;
    [SerializeField] private PickableData requiredItem;
    
    [Tooltip("ID is used when output expects multiple inputs to be pressed to determine which one was already toggled. " +
             "ID's can repeat in a scene but should not repeat between inputs that share a single output interaction.")]
    [SerializeField] private int interactionID = 0;
    
    [Tooltip("Trigger: interaction sends a single signal to output. " +
             "Toggle: interaction sends two different signals. On toggle ON and OFF.")]
    [SerializeField] private InteractionType interactionType;
    [Header("Only used if InteractionType is set to 'Toggle'")]
    [Tooltip("Initial state of toggle. Only used when InteractionType is set to 'Toggle'.")]
    [SerializeField] private ToggleState toggleState = ToggleState.Off;
    
    [Header("Events\nNOTE: Use 'Dynamic' methods from top of the list.")]
    [SerializeField] private InteractionEvent interactionEvent;
    [Header("Off Event only used with InteractionType 'Toggle'")]
    [SerializeField] private InteractionEvent interactionOffEvent;

    private void Start()
    {
        // set the initial state of toggle
        if (interactionType == InteractionType.Toggle)
        {
            Interact(null); 
        }
    }

    public string GetTip()
    {
        return tip;
    }
    
    public string GetAltTip()
    {
        return altTip;
    }
    
    public bool Interact(NewInventory inv)
    {
        if (canInteract)
        {
            // KB - Added checking for required item
            if (requiredItem != null)
            {
                if (!inv.itemExists(requiredItem))
                {
                    return false;
                }
            }

            if (interactionType == InteractionType.Trigger)
            {
                interactionEvent?.Invoke(interactionID);
                if (singleUse)
                {
                    canInteract = false;
                }
            }
            else
            {
                Debug.Log("Toggle state changed!");
                switch (toggleState)
                {
                    case ToggleState.Off:
                        toggleState = ToggleState.On;
                        interactionOffEvent?.Invoke(interactionID);
                        break;
                        
                    case ToggleState.On:
                        toggleState = ToggleState.Off;
                        interactionEvent?.Invoke(interactionID);
                        break;
                }
                if (singleUse)
                {
                    canInteract = false;
                }
            }

            return true;
        }
        return false;
    }
}
