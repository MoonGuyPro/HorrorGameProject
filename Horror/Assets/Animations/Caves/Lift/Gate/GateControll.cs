using DG.Tweening;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateControll : MonoBehaviour
{
    [SerializeField] private Transform _rightDoor;
    [SerializeField] private Transform _leftDoor;

    [SerializeField] private float _startRotationRight;
    [SerializeField] private float _startRotationLeft;

    [SerializeField] private float _endRotationRight;
    [SerializeField] private float _endRotationLeft;

    [SerializeField] private float _rotationDuration;

    public float RotationDuration { get => _rotationDuration; }

    [SerializeField]
    private EventReference _gateOpenSound;
    [SerializeField]
    private EventReference _gateCloseSound;

    private void Awake()
    {
        _rightDoor = transform.GetChild(0);
        _leftDoor = transform.GetChild(1);
    }

    private void PlayAnimation(float rotationLeft, float rotationRight)
    {
        DOTween.Kill(transform);

        if (_rightDoor)
        {

            _rightDoor.DOLocalRotate(new Vector3(_rightDoor.localEulerAngles.x, rotationRight, _rightDoor.localEulerAngles.z), _rotationDuration);
        }
        if (_leftDoor)
        {
            _leftDoor.DOLocalRotate(new Vector3(_leftDoor.localEulerAngles.x, rotationLeft, _leftDoor.localEulerAngles.z), _rotationDuration);
        }
    }

    public void OpenGate()
    {
        PlayAnimation(_endRotationLeft, _endRotationRight);
        RuntimeManager.PlayOneShot(_gateOpenSound, transform.position);
    }

    public void CloseGate()
    {
        PlayAnimation(_startRotationLeft, _startRotationRight);
        RuntimeManager.PlayOneShot(_gateCloseSound, transform.position);
    }
}
