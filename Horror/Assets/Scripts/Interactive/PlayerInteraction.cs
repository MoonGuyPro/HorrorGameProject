using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform PlayerCamera;
    [Header("Max distance of interaction")]
    public float MaxDistance = 5;

    private Interactive interactive;

    void Update()
    {
        // On interact key
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, MaxDistance))
            {
                // Is it interactive object?
                if (hit.transform.tag == "Interactive")
                {
                    // Call interaction
                    interactive = hit.transform.GetComponentInParent<Interactive>();
                    interactive.interact();
                }
            }
        }
    }
}
