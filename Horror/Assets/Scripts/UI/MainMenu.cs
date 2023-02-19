using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private EventReference hover;
    [SerializeField]
    private EventReference press;

    [SerializeField] 
    private Button continueButton;
    
    void Start()
    {
        print(PlayerPrefs.GetString("lastLevel", null));
        if (PlayerPrefs.GetString("lastLevel", null) == "")
        {
            continueButton.interactable = false;
        }
        Cursor.visible = true;
    }

    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("lastLevel"));
    }
    
    public void NewGame() 
    {
       SceneManager.LoadScene("VideoPlayer");
    }

    public void QuitGame()
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
