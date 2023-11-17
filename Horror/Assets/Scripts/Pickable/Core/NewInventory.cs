using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInventory : MonoBehaviour
{
    private List<PickableData> items;

    private void Start()
    {
        items = new List<PickableData>();
    }

    public void addItem(PickableData item)
    {
        items.Add(item);
    }

    public bool removeItem(PickableData item)
    {
        if (items.Contains(item))
        {
            return items.Remove(item);
        }
        return false;
        // foreach (PickableData item in items)
        // {
        //     if (item.name == name)
        //     {
        //          return items.Remove(item);
        //     }
        // }
        // return false;
    }

    public bool itemExists(PickableData item)
    {
        return items.Contains(item);
        // foreach (PickableData item in items)
        // {
        //     if (item.name == name)
        //     {
        //         return true;
        //     }
        // }
        // return false;
    }

    public void print()
    {
        foreach(PickableData item in items)
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
        foreach (PickableData item in items)
        {
            output += (item.DisplayName + "\n");
        }
        return output;
    }
}
