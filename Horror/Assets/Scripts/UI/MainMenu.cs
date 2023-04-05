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
    
    void Awake() 
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // I tried - Kris
    }
    
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
        //Debug.Log("Quitting game...");
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

    //Go to survey
    public void GoToSurvey()
    {
        Application.OpenURL("https://forms.gle/YhW9yuia6bWT5cXS9");
    }

}
