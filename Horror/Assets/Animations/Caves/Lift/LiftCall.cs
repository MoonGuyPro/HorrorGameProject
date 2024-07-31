using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum LiftPos
{
    Up,
    Down
}

public class LiftCall : MonoBehaviour
{
    [SerializeField] private LiftPos _callTo;

    [SerializeField] private float _liftUp;
    [SerializeField] private float _liftDown;

    [SerializeField] private Animator _liftAnimator;
    private AnimationClip _clipUp;
    private AnimationClip _clipDown;

    /* How long does the lift drive takes (Change also tranistion duration in animator)*/
    [SerializeField] private float _liftDuration;


    private void Start()
    {
        _liftAnimator = GetComponent<Animator>();
        CreateAnimationClips();
    }
    private void CreateAnimationClips()
    {
        // Up Animation Clip
        _clipUp = new AnimationClip();
        Keyframe[] keysUp = new Keyframe[2];
        keysUp[0] = new Keyframe(0.0f, transform.localPosition.z);
        keysUp[1] = new Keyframe(_liftDuration, _liftUp);
        AnimationCurve curveUp = new AnimationCurve(keysUp);
        _clipUp.SetCurve("", typeof(Transform), "localPosition.z", curveUp);

        // Down Animation Clip
        _clipDown = new AnimationClip();
        Keyframe[] keysDown = new Keyframe[2];
        keysDown[0] = new Keyframe(0.0f, transform.localPosition.z);
        keysDown[1] = new Keyframe(_liftDuration, _liftDown);
        AnimationCurve curveDown = new AnimationCurve(keysDown);
        _clipDown.SetCurve("", typeof(Transform), "localPosition.z", curveDown);

        var overrideController = new AnimatorOverrideController(_liftAnimator.runtimeAnimatorController);
        
        if (_callTo == LiftPos.Down)
        {
            overrideController["DefaultState"] = _clipDown;
            overrideController["DefaultAnimation"] = _clipUp;
        }
        if (_callTo == LiftPos.Up)
        {
            overrideController["DefaultState"] = _clipUp;
            overrideController["DefaultAnimation"] = _clipDown;
        }

        _liftAnimator.runtimeAnimatorController = overrideController;
    }

    public void LiftUp()
    {
        if (_callTo.Equals(LiftPos.Up))
            return;
        _callTo = LiftPos.Up;
        PlayAnimation();
    }

    public void LiftDown()
    {
        if (_callTo.Equals(LiftPos.Down))
            return;
        _callTo = LiftPos.Down;
        PlayAnimation();
    }

    public void LiftToOther()
    {
        if (_callTo.Equals(LiftPos.Down))
        {
            LiftUp();
        }
        else
        if (_callTo.Equals(LiftPos.Up))
        {
            LiftDown();
        }
    }

    private void PlayAnimation()
    {
        if (_liftAnimator.IsInTransition(0))
            return;
        _liftAnimator.SetTrigger("PlayAnimation");
    }
}
