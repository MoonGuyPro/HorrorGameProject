using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class SaveGame : MonoBehaviour
{
    void Start()
    {
        // On scene change save last level's name
        HandleSaveFile.SaveProgress(SceneManager.GetActiveScene().name);
    }
}
