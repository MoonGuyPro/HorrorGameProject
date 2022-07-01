using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayLevel1() 
    {
        SceneManager.LoadScene("Level1");
    }

    public void PlayLevel2() 
    {
        SceneManager.LoadScene("EasyMechanics");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
