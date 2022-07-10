using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHole : InputLogic
{
    public int changed;
    public Inventory inv;
    public string keyName;
    protected override void Behavior()
    {
        //inv.print();
        if (inv.itemExists(keyName))
        {
            changed ^= 1;
        }
    }

    public override bool Interact()
    {
        //print("sdfdssdff");
        Behavior(); // Call input behavior (implemented in extended class)

        if (inv.itemExists(keyName))
        {
            active = !active;
            // output can be null if there is no output object.
            // for example click to trigger audio.
            if (output != null)
            {
                output.CheckState(); // Check state of output
            }
            return true;
        }
        return false;
    }
}
