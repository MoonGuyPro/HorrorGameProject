using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Supercube : Pickable
{
    [Header("Supercube")] 
    [SerializeField] private string nextLevel;
    [SerializeField] private TransitionTweening transitionTweening;

    public override void interact()
    {
        Debug.Log("Picked.");
        StartCoroutine(FadeAndChangeLevel());
        GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    IEnumerator FadeAndChangeLevel()
    {
        transitionTweening.FadeOut();
        yield return new WaitForSeconds(transitionTweening.fadeoutTime);
        SceneManager.LoadScene(nextLevel);
    }
}
