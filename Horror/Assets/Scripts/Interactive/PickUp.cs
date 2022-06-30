using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform theDest;
    bool isHold;

    private void OnMouseDown()
    {
        isHold = true;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        transform.position = theDest.position;
        transform.parent = GameObject.Find("Destination").transform;
    }

    private void Update()
    {
        if (isHold)
        {
            transform.position = theDest.position;
        }
    }

    private void OnMouseUp()
    {
        isHold = false;
        transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = true;

    }
}
