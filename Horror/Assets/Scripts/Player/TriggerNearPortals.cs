using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNearPortals : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Portal>() != null)
        {
            Debug.Log("Entered portal: " + other.name);
            other.GetComponent<Portal>().ChangeResolution(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Portal>() != null)
        {
            Debug.Log("Exited portal: " + other.name);
            other.GetComponent<Portal>().ChangeResolution(false);
        }
    }
}
