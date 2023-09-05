using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Supercube : Pickable
{
    [Header("Supercube")] 
    [SerializeField] private string nextLevel;
    [SerializeField] private TransitionTweening transitionTweening;
    [SerializeField] private MeshRenderer model;

    public override void interact()
    {
        model.enabled = false; // I don't work lol
        RuntimeManager.PlayOneShot(pickUpSound);
        StartCoroutine(FadeAndChangeLevel());
    }

    IEnumerator FadeAndChangeLevel()
    {
        transitionTweening.FadeOut();
        yield return new WaitForSeconds(transitionTweening.fadeoutTime);
        SceneManager.LoadScene(nextLevel);
    }
}
