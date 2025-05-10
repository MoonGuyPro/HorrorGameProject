using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    async public void FadeOutAndInBlack()
    {
        FadeoutBlack();
        await Task.Delay(1000);
        FadeinBlack();
    }
}
