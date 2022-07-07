using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Pickable> items;

    // Start is called before the first frame update
    private void Start()
    {
        items = new List<Pickable>();
    }

    public void addItem(Pickable item)
    {
        items.Add(item);
    }

    public bool removeItem(string name)
    {
        foreach (Pickable item in items)
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
        foreach (Pickable item in items)
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
        foreach(Pickable item in items)
        {
            print(item.name);
        }
    }
}
