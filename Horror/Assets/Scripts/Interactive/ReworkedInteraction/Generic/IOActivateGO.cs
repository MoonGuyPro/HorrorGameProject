using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOActivateGO : MonoBehaviour
{
    [SerializeField] bool bEnableOnStart = true;

    void Start()
    {
        gameObject.SetActive(bEnableOnStart);
    }

    public void OnToggleOn()
    {
        gameObject.SetActive(true);
    }

    public void OnToggleOff()
    {
        gameObject.SetActive(false);
    }
}
