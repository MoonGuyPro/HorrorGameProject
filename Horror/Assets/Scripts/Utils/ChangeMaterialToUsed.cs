using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialToUsed : MonoBehaviour
{
    [Header("Change to this material when was used")]
    [SerializeField] private Material usedMaterial;

    public void ChangeMaterial()
    {
        if (usedMaterial != null)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                Debug.Log("Applying material to: " + rend.gameObject.name);
                rend.material = usedMaterial;
            }
        }
    }
}
