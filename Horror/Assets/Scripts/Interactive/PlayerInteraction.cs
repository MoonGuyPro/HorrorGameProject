using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform PlayerCamera;
    [Header("Max distance of interaction")]
    public float MaxDistance = 5;

    private InputLogic input;

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
                    // Get InputLogic and change logic state
                    input = hit.transform.GetComponentInParent<InputLogic>();
                    input.toggle();
                }
            }
        }
    }
}
