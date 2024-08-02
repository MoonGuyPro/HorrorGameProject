using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum LiftPos
{
    Up,
    Down
}

public class LiftCall : MonoBehaviour
{
    [Tooltip("Position where the lift goes")]
    [SerializeField] private LiftPos _callTo;

    [Tooltip("Lift position when on the top")]
    [SerializeField] private float _liftUp;

    [Tooltip("Lift position when on the bottom")]
    [SerializeField] private float _liftDown;

    [Tooltip("Lift's animator")]
    [SerializeField] private Animator _liftAnimator;

    /** Lift's clip with position on the top */
    private AnimationClip _clipUp;
    /** Lift's clip with position on the bottom */
    private AnimationClip _clipDown;

    [Tooltip("How long does the lift drive takes (Read-Only)")]
    [SerializeField] private float _liftDuration;

    [Tooltip("Lift's speed")]
    [Range(0,10)]
    [SerializeField] private float _liftSpeed;


    private void Start()
    {
        _liftAnimator = GetComponent<Animator>();
        CreateAnimationClips();
    }

    private void CreateAnimationClips()
    {
        // Count lift drive duration
        _liftDuration = Math.Abs(_liftDown - _liftUp) / _liftSpeed;

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

        if (_liftAnimator.GetCurrentAnimatorStateInfo(0).IsName("FirstLiftPos"))
            _liftAnimator.CrossFadeInFixedTime("SecondLiftPos", _liftDuration);
        else
            _liftAnimator.CrossFadeInFixedTime("FirstLiftPos", _liftDuration);
    }
}
