using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPortal : MonoBehaviour
{
    public int levelNr;
    public RawImage[] cube;

    private void Start()
    {
        for(int i = 0; i < levelNr - 1; i++)
        {
            cube[i].enabled = true;
        }
    }

    public void getCube()
    {
        cube[levelNr - 1].enabled = true;
    }
}
