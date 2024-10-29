using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerAsUnityEvent : MonoBehaviour
{
    [SerializeField] 
    private bool oneshot = true;
    public bool Oneshot {get => oneshot;}

    public UnityEvent OnTrigger;

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            OnTrigger.Invoke();

            if (oneshot)
            {
                enabled = false;
            }
        }
    }

    void OnDrawGizmos() {
        // Draw a semitransparent red cube at the transforms position
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(transform.position, boxCollider.size);
    }
}
