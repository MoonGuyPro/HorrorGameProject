using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCondition : JustCondition
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            value = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            value = false;
        }
    }

    public override void ReCheckTrigger()
    {
        if (player != null)
        {
            if (collider.bounds.Contains(player.transform.position))
            {
                value = true;
            }
            else { value = false; }
        }
    }
}
