using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMODCredits : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(ChangeToMainMenu());
    }

    IEnumerator ChangeToMainMenu()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("MainMenuScene");
    }
}
