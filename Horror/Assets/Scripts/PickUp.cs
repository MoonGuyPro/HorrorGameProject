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
        this.transform.position = theDest.position;
        this.transform.parent = GameObject.Find("Destination").transform;
    }

    private void Update()
    {
        if (this.isHold)
        {
            this.transform.position = this.theDest.position;
        }
    }

    private void OnMouseUp()
    {
        this.isHold = false;
        this.transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = true;

    }
}
