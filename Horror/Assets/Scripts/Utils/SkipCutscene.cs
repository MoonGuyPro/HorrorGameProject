using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkipCutscene : MonoBehaviour
{
    [Header("Animator Settings")]
    public Animator animator;
    public string cutsceneStateName;
    public int skipToFrame = 120;
	public float framerate = 24f;

    [Header("Input Settings")]
    public InputActionReference skipAction;
    public float holdDuration = 2f;

    private float holdTimer = 0f;
    private bool hasSkipped = false;

    void OnEnable()
    {
        if (skipAction)
		{
            skipAction.action.Enable();
		}
    }

    void OnDisable()
    {
        if (skipAction)
		{
            skipAction.action.Disable();
		}
    }

    void Update()
    {
        if (hasSkipped || skipAction == null) 
			return;

        if (skipAction.action.IsPressed())
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdDuration)
            {
                SkipAnim();
            }
        }
        else
        {
            holdTimer = 0f;
        }
    }

    void SkipAnim()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(cutsceneStateName))
        {	
            float skipTime = skipToFrame / framerate;
            float clipLength = stateInfo.length;
            float normalizedTime = Mathf.Clamp01(skipTime / clipLength);

            animator.Play(cutsceneStateName, 0, normalizedTime);
            animator.Update(0f);
        }

        hasSkipped = true;
    }
}
