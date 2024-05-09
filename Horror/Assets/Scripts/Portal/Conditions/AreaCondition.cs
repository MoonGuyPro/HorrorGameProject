using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCondition : JustCondition
{
    private BoxCollider _area;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            value = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            value = false;
        }
    }
}
