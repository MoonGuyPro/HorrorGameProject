using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingImage : MonoBehaviour
{
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // Read pixels from the source RenderTexture, apply the material, copy the updated results to the destination RenderTexture
        Graphics.Blit(src, dest);
    }
}
