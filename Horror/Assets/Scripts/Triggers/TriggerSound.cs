using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    public bool oneShot = true; // Should it trigger only once?
    [SerializeField] private EventReference eventReference;
    [SerializeField] private GameObject soundSourceGameObject;

    void OnTriggerEnter(Collider player)
    {
        GameObject source = soundSourceGameObject == null ? gameObject : soundSourceGameObject;
        if (player.gameObject.tag == "Player" && enabled)
        {
            RuntimeManager.PlayOneShotAttached(eventReference, source);
            if (oneShot)
            {
                enabled = false;
            }
        }
    }
}
