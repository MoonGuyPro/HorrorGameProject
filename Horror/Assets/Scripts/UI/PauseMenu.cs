using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using UnityEngine.InputSystem;
using System;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject pauseMenu;
    public GameObject PauseMenuUI;
    public GameObject options;
    public GameObject overlay;

    [SerializeField]
    private EventReference hover;
    
    [SerializeField]
    private EventReference press;
    
    void OnEnable ()
    {
		InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
		InputActionMap inputActionMap = inputActionAsset.FindActionMap("Player");
        inputActionMap.FindAction("Pause").performed += Pause;
    }

    void OnDisable ()
    {
		InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
		InputActionMap inputActionMap = inputActionAsset.FindActionMap("Player");
        inputActionMap.FindAction("Pause").performed -= Pause;
    }

    private void Update()
    {
        // if (!Input.GetButtonDown("Pause")) return;
        // if (options.activeSelf) return; // do not unpause when in options
            
        // if (IsPaused)
        // {
        //     Resume();
        // }
        // else
        // {
        //     Pause();
        // }
    }

    void Pause(InputAction.CallbackContext context)
    {
        // Do not unpause when in options
        if (options.activeSelf)
            return;

        IsPaused =! IsPaused;
        if (IsPaused)
            Pause();
        else
            Resume();
    }

    private void Pause() 
    {
        PauseMenuUI.SetActive(true);
        overlay.SetActive(true);
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        IsPaused = true;
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        overlay.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsPaused = false;
    }

    public void MainMenu()
    {
		SceneManager.LoadScene("MainMenuScene");
        Resume();
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if(player) { Destroy(player); }
    }

    public void Quit() 
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
    
    public void PlayHoverSound()
    {
        RuntimeManager.PlayOneShot(hover);
    }

    public void PlayPressSound()
    {
        RuntimeManager.PlayOneShot(press);
    }
}
