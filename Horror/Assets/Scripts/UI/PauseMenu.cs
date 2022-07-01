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
            MainMenu();
        }
    }
            /*if (IsPaused) 
            {
                Resume();
            }
            else
            {
                Pause();
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
    }*/

    public void MainMenu()
    {
        //Resume();
        SceneManager.LoadScene("MainMenu");
    }

    /*public void Quit() 
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }*/
}
