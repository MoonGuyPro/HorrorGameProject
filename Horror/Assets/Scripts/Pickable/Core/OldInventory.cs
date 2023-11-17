using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldInventory : MonoBehaviour
{
    private List<OldPickable> items;

    private void Start()
    {
        items = new List<OldPickable>();
    }

    public void addItem(OldPickable item)
    {
        items.Add(item);
    }

    public bool removeItem(string name)
    {
        foreach (OldPickable item in items)
        {
            if (item.name == name)
            {
                 return items.Remove(item);
            }
        }
        return false;
    }

    public bool itemExists(string name)
    {
        foreach (OldPickable item in items)
        {
            if (item.name == name)
            {
                return true;
            }
        }
        return false;
    }

    public void print()
    {
        foreach(OldPickable item in items)
        {
            print(item.name);
        }
    }

    // returns concatenated text of names of all inventory items to be displayed on UI
    public string printInGameNames()
    {
        if (items.Count == 0)
        {
            return "Empty";
        }
        string output = "";
        foreach (OldPickable item in items)
        {
            output += (item.name + "\n");
        }
        return output;
    }
}
