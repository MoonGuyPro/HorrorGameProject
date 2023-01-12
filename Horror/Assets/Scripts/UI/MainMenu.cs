using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Start()
    {
        Cursor.visible = true;
    }

    public void PlayLevel1() 
    {
       SceneManager.LoadScene("VideoPlayer");
    }

    public void PlayLevel2() 
    {
        SceneManager.LoadScene("EasyMechanics");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
