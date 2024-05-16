using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ComicCutscene : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI bottomText;

    private bool paused = false;
    private bool finished = false;

    void Start()
    {
        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
		InputActionMap inputActionMap = inputActionAsset.FindActionMap("UI");
        inputActionMap.FindAction("Any").performed += OnAnyKey;
    }

    void OnDestroy() 
    {
        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
		InputActionMap inputActionMap = inputActionAsset.FindActionMap("UI");
        inputActionMap.FindAction("Any").performed -= OnAnyKey;
    }

    private void OnAnyKey(InputAction.CallbackContext context)
    {
        if (finished)
        {
            SceneManager.LoadScene("Labs");
        } 
        else if (paused)
        {
            animator.speed = 1.0f;
            paused = false;
        }
        else
        {
            animator.speed = 10.0f;
        }
    }

    void OnBreak()
    {
        animator.speed = 0.0f;
        paused = true;
    }

    void OnFinish()
    {
        finished = true;
    }
}
