using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ComicCutscene : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI bottomText;

    // Start is called before the first frame update
    void Start()
    {
        bottomText.enabled = false;

        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
		InputActionMap inputActionMap = inputActionAsset.FindActionMap("UI");
        inputActionMap.FindAction("Any").performed += OnContinue;
    }

    private void OnContinue(InputAction.CallbackContext context)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            animator.speed = 1.0f;
            bottomText.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBreak()
    {
        animator.speed = 0.0f;
        bottomText.enabled = true;
    }
}
