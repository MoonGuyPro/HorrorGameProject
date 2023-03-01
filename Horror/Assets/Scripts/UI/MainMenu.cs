using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Utils;
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
        print(HandleSaveFile.LoadProgress());
        if (HandleSaveFile.LoadProgress() == "")
        {
            continueButton.interactable = false;
        }
        
        // DONT TOUCH THESE!!!
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Continue()
    {
        SceneManager.LoadScene(HandleSaveFile.LoadProgress());
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
