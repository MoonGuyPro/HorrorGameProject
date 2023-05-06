using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPortal : MonoBehaviour
{
    public RawImage cube1;
    public RawImage cube2;
    public RawImage cube3;
    public RawImage cube4;
    public RawImage cube5;
    public RawImage cube6;

    private void Start()
    {
        Debug.Log("Start");
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            cube1.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            cube2.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            cube3.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            cube4.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            cube5.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            cube6.enabled = true;
        }

    }

}
