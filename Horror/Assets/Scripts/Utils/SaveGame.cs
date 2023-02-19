using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour
{
    void Start()
    {
        // On scene change save last level's name
        PlayerPrefs.SetString("lastLevel", SceneManager.GetActiveScene().name);
    }
}
