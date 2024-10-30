using System.Collections.Generic;
using UnityEngine;

// Use this trigger to set objects as active/inactive when player enters
public class TriggerShowHide : MonoBehaviour
{
    public bool oneShot = true; // Should it trigger only once?
    public List<GameObject> objectsToShow;
    public List<GameObject> objectsToHide;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (objectsToShow != null)
            {
                foreach (GameObject go in objectsToShow)
                {
                    go.SetActive(true);
                }
            }

            if (objectsToHide != null)
            {
                foreach (GameObject go in objectsToHide)
                {
                    go.SetActive(false);
                }
            }

            // Disable trigger if it's oneshot
            if (oneShot)
            {
                enabled = false;
            }
        }
    }
    void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        // sometimes we need more thanone collider on object so we iterate through all of them
        foreach (var col in colliders)
        {
            Gizmos.DrawCube(transform.position + col.center, col.size);
        }
    }
}