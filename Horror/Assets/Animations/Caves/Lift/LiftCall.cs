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

    [Tooltip("How long does the lift drive takes (Read-Only)")]
    [SerializeField] private float _liftDuration;

    [Tooltip("Lift's speed")]
    [Range(0,10)]
    [SerializeField] private float _liftSpeed;

    [SerializeField] private AnimationCurve _liftCurve;

    private Vector3 _targetPosition;

    private void Start()
    {
        CreateAnimationParams();
    }

    private void CreateAnimationParams()
    {
        // Count lift drive duration
        _liftDuration = Math.Abs(_liftDown - _liftUp) / _liftSpeed;
    }

    public void LiftUp()
    {
        if (_callTo.Equals(LiftPos.Up))
            return;
        _callTo = LiftPos.Up;
        _targetPosition.z = _liftUp;
        PlayAnimation();
    }

    public void LiftDown()
    {
        if (_callTo.Equals(LiftPos.Down))
            return;
        _callTo = LiftPos.Down;
        _targetPosition.z = _liftDown;
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
        DOTween.Kill(transform);
        transform.DOLocalMoveZ(_targetPosition.z, _liftDuration).SetEase(_liftCurve).SetUpdate(UpdateType.Fixed);
    }
}
