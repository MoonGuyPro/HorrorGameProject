using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [SerializeField]
	private Animator transitionAnim;

    public void EndLevel()
    {
        transitionAnim.SetTrigger("Start");
    }

    public void FadeoutBlack()
    {
        transitionAnim.SetBool("FadeBlack", true);
    }

    public void FadeinBlack()
    {
        transitionAnim.SetBool("FadeBlack", false);
    }
}
