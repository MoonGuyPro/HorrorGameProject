using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Axis
{
    X,
    Y,
    Z
}

public class ShaderTest : MonoBehaviour
{
    // get material 
    private Material material;

    [SerializeField] 
    private Axis axis;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        material.DisableKeyword("_DISPLACEMENTAXIS_X");
        switch (axis)
        {
            case Axis.X:
                material.EnableKeyword("_DISPLACEMENTAXIS_X");
                break;
            case Axis.Y:
                material.EnableKeyword("_DISPLACEMENTAXIS_Y");
                break;
            case Axis.Z:
                material.EnableKeyword("_DISPLACEMENTAXIS_Z");
                break;
        }
    }

    private void Update()
    {
        var playerPosition = PlayerInstance.GetCameraPosition();
        
        // set material's vector property
        material.SetVector("_playerPosition", playerPosition);
    }
}
