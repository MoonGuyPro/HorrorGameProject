using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiPortalTrigger : MonoBehaviour
{
    [SerializeField]
    private bool enablePortalsOnEnter = true;

    [SerializeField]
    private List<GameObject> portals = new();

    void OnCollisionEnter(Collision collision)
    {
        print("entered!");
        foreach (GameObject portal in portals) 
        {
            if (portal == null) continue;
            portal.SetActive(enablePortalsOnEnter);
        }
    }
}
