using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject PauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (IsPaused)
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Pause();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    void Pause() 
    {
        Debug.Log("Paused!");
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        IsPaused = true;
    }

    public void Resume()
    {
        Debug.Log("Resumed!");
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
}
