using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class OutputInteraction : MonoBehaviour
{
    [SerializeField] private LogicGate logicType = LogicGate.AND;
    [SerializeField] private int inputAmounts;
    [SerializeField] private bool invert = false;

    // curent output state
    [SerializeField] private BinaryState active = BinaryState.OFF;
    
    private List<Signal> signals = new List<Signal>();
    
    [Header("Events")]
    [SerializeField] private UnityEvent onEvent;
    [SerializeField] private UnityEvent offEvent;

    public void OnInteraction(int id)
    {
        Debug.Log("recieved ID: " + id);
        // check if a signal with given id already exists
        int index = signals.FindIndex(signal => signal.GetID() == id);
        // if no, add new signal with state ON
        if (index == -1)
        {
            signals.Add(new Signal(BinaryState.ON, id));
        }
        else
        {
            // if yes, change state to ON
            signals[index] = new Signal(BinaryState.ON, id);
        }
        
        switch (logicType)
        {
            case LogicGate.AND:
                active = (And() != invert) ? BinaryState.ON : BinaryState.OFF ;
                break;
            case LogicGate.OR:
                active = (Or() != invert) ? BinaryState.ON : BinaryState.OFF;
                break;
            case LogicGate.XOR:
                active = (Xor() != invert) ? BinaryState.ON : BinaryState.OFF;
                break;
        }
        SendEvents();
    }

    public void OffInteraction(int id)
    {
        Debug.Log("recieved ID: " + id);
        // check if a signal with given id already exists
        int index = signals.FindIndex(signal => signal.GetID() == id);
        
        // if no, add new signal with state OFF
        if (index == -1)
        {
            signals.Add(new Signal(BinaryState.OFF, id));
        }
        else
        {
            // if yes, change state to OFF
            signals[index] = new Signal(BinaryState.OFF, id);
        }
        
        switch (logicType)
        {
            case LogicGate.AND:
                active = (And() != invert) ? BinaryState.ON : BinaryState.OFF ;
                break;
            case LogicGate.OR:
                active = (Or() != invert) ? BinaryState.ON : BinaryState.OFF;
                break;
            case LogicGate.XOR:
                active = (Xor() != invert) ? BinaryState.ON : BinaryState.OFF;
                break;
        }
        
        SendEvents();
    }

    private void SendEvents()
    {
        if (active == BinaryState.ON)
        {
            onEvent.Invoke();
        }
        else
        {
            offEvent.Invoke();
        }
    }

    private bool And()
    {
        return signals.TrueForAll(signal => signal.state == BinaryState.ON) && signals.Count == inputAmounts;
    }

    private bool Or()
    {
        foreach (Signal signal in signals)
        {
            if (signal.state == BinaryState.ON)
            {
                return true;
            }
        }
        return false;
    }

    private bool Xor()
    {
        return signals.FindAll(signal => signal.state == BinaryState.ON).Count % 2 == 1;
    }
}
