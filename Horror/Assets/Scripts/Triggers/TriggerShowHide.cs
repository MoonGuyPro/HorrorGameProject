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
                        foreach(GameObject go in objectsToShow)
                        {
                                go.SetActive(true);
                        }

                        foreach (GameObject go in objectsToHide)
                        {
                                go.SetActive(false);
                        }
                        
                        // Disable trigger if it's oneshot
                        if (oneShot)
                        {
                                enabled = false;
                        }
                }
        }
}