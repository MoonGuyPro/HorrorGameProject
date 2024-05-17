using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ReversedAreaCondition : JustCondition
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            value = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            value = true;
        }
    }

    public override void ReCheckTrigger()
    {
        if (player != null)
        {
            if (collider.bounds.Contains(player.transform.position))
            {
                value = false;
            }
            else { value = true; }
        }
    }
}
