using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float maxDistance = 5;
    [SerializeField] private float radius = 0.45f;

    void Update()
    {
        if (!Input.GetButtonDown("Scan")) return;

        RaycastHit hit;
        if (Physics.SphereCast(playerCamera.position, radius, playerCamera.forward, out hit, maxDistance))
        {
            if (hit.transform.CompareTag("Scannable"))
            {
                Scannable scanable = hit.transform.GetComponentInParent<Scannable>();

                if (scanable is not null)
                {
                    if (scanable.Data is not null)
                    {
                        print(scanable.Data.name);
                    }
                    else
                    {
                        Debug.LogWarning("Scanable has no data!");
                    }
                }
                else
                {
                    Debug.LogWarning("Scanable component is missing!");
                }
            }
        } 
    }
}
