using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEditor.Experimental.GraphView.GraphView;

public enum LiftPos
{
    Up,
    Down
}

public class LiftCall : MonoBehaviour
{
    [Header("Lift parameters")]
    [Tooltip("Position where the lift goes")]
    [SerializeField] private LiftPos _callTo;

    [Tooltip("Lift position when on the top")]
    [SerializeField] private float _liftUp;

    [Tooltip("Lift position when on the bottom")]
    [SerializeField] private float _liftDown;

    [Tooltip("How long does the lift drive takes (Read-Only)")]
    [SerializeField] private float _liftDuration;

    [Tooltip("Lift's speed")]
    [Range(0, 10)]
    [SerializeField] private float _liftSpeed;

    [SerializeField] private bool _isMoving = false;

    [SerializeField] private AnimationCurve _liftCurve;

    private Vector3 _targetPosition;

    [Space(8)]
    [SerializeField] private bool _autoStart = false;


    [Header("Lift's gates")]
    [SerializeField] List<GateControll> _gatesOpenUp;
    [SerializeField] List<GateControll> _gatesOpenDown;


    [Tooltip("Lift's deley after gates close")]
    [SerializeField] float _delay = 0.5f;

    [SerializeField] StudioEventEmitter elevatorSoundEmitter;
    private bool isPastStoppingPoint = false;

    private void Start()
    {
        CreateAnimationParams();
        OpenGates();
        
        if (_autoStart) 
            LiftToOther();
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
        if (_isMoving)
            return;
        if (_callTo.Equals(LiftPos.Down))
        {
            LiftUp();
            _isMoving = true;
        }
        else
        if (_callTo.Equals(LiftPos.Up))
        {
            LiftDown();
            _isMoving = true;
        }
    }

    private void PlayAnimation()
    {
        CloseGates();
        float delay = _delay;
        if (_gatesOpenDown.Count > 0)
            delay += _gatesOpenDown[0].RotationDuration;

        DOTween.Kill(transform);
        isPastStoppingPoint = false;
        var tween = transform.DOLocalMoveZ(_targetPosition.z, _liftDuration)
            .SetDelay(delay)
            .SetEase(_liftCurve)
            .SetUpdate(UpdateType.Fixed)
            .OnStart(() =>
            {
                elevatorSoundEmitter.Play();
            })
            .OnComplete(() =>
            {
                OpenGates();
                _isMoving = false;
            });
        tween.OnUpdate(() =>
        {
            if (tween.position / _liftDuration > 0.85f && !isPastStoppingPoint)
            {
                isPastStoppingPoint = true;
                elevatorSoundEmitter.Stop();
            }
        });
    }

    private void OpenGates()
    {
        if (_callTo.Equals(LiftPos.Down))
        {
            foreach (GateControll gate in _gatesOpenDown)
            {
                if (gate)
                    gate.OpenGate();
            }
        }
        else if (_callTo.Equals(LiftPos.Up))
        {
            foreach (GateControll gate in _gatesOpenUp)
            {
                if (gate)
                    gate.OpenGate();
            }
        }
    }

    private void CloseGates()
    {
        foreach (GateControll gate in _gatesOpenUp)
        {
            if (gate)
                gate.CloseGate();
        }
        foreach (GateControll gate in _gatesOpenDown)
        {
            if (gate)
                gate.CloseGate();
        }
    }
}
