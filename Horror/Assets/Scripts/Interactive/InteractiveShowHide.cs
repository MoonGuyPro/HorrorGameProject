using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveShowHide : Interactive
{
    public List<GameObject> objectsToShowHide;

    public override bool Interact(OldInventory inv)
    {
        if (objectsToShowHide != null)
        {
                foreach(GameObject go in objectsToShowHide)
                {
                        go.SetActive(!go.active);
                }       
        }

        return true;
    }
}
