using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannableManager : MonoBehaviour
{
    public Material overlayInstance;
    // Start is called before the first frame update
    void Awake()
    {
        // Znajd� wszystkie obiekty w scenie
        Scannable[] scannables = FindObjectsOfType<Scannable>();

        // Przejd� przez ka�dy znaleziony obiekt

        foreach (var scannable in scannables)
        {
            // Ustaw overlayInstance w ka�dym obiekcie
            scannable.overlayMaterial = overlayInstance;
        }
    }


}
