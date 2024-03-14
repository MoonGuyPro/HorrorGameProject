using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuntimeInitialize : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RuntimeInitializeOnLoadMethods()
    {
        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
        if (inputActionAsset != null)
        {
            inputActionAsset.Enable();
        }
        else
        {
            Debug.LogError("InputActionAsset called NyctoInputActions is missing in resources folder!");
        }
    }
}
