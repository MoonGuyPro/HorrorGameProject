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
    
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1;
    }
    
    void Start()
    {
        print(HandleSaveFile.LoadProgress());
        if (HandleSaveFile.LoadProgress() == "")
        {
            continueButton.interactable = false;
        }
        Cursor.visible = true;
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
