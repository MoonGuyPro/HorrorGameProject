using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannableManager : MonoBehaviour
{
    public Material overlayInstance;
    // Start is called before the first frame update
    void Awake()
    {
        // Znajdü wszystkie obiekty w scenie
        Scannable[] scannables = FindObjectsOfType<Scannable>();

        // Przejdü przez kaødy znaleziony obiekt

        foreach (var scannable in scannables)
        {
            // Ustaw overlayInstance w kaødym obiekcie
            scannable.overlayMaterial = overlayInstance;
        }
    }


}
