using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject PauseMenuUI;
    
    [SerializeField]
    private EventReference hover;
    [SerializeField]
    private EventReference press;

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (!IsPaused)
        {
            Pause();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Resume();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Pause() 
    {
        //Debug.Log("Paused!");
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        IsPaused = true;
    }

    public void Resume()
    {
        //Debug.Log("Resumed!");
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
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
