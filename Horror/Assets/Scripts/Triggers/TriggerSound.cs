using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    public bool oneShot = true; // Should it trigger only once?
    [SerializeField] private EventReference eventReference;

    void Start()
    {

    }

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            RuntimeManager.PlayOneShotAttached(eventReference, gameObject);
            if (oneShot)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
