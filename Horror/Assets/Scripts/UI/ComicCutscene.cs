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
    [SerializeField] float animationSpeed = 0.5f;
    [SerializeField] float fastForwardSpeed = 10.0f;
    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI bottomText;

    private InputAction anyKeyAction;
    private bool paused = false;
    private bool finished = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        animator.speed = animationSpeed;

        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
		InputActionMap inputActionMap = inputActionAsset.FindActionMap("UI");
        anyKeyAction = inputActionMap.FindAction("Any");
        anyKeyAction.performed += OnAnyKeyPerformed;
        anyKeyAction.canceled += OnAnyKeyCanceled;
    }

    void OnDestroy() 
    {
        anyKeyAction.performed -= OnAnyKeyPerformed;
        anyKeyAction.canceled -= OnAnyKeyCanceled;
    }

    private void OnAnyKeyPerformed(InputAction.CallbackContext context)
    {
        if (finished)
        {
            SceneManager.LoadScene("Labs");
        } 
        else
        {
            animator.speed = fastForwardSpeed;
        }
    }

    private void OnAnyKeyCanceled(InputAction.CallbackContext context)
    {
        animator.speed = animationSpeed;
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
