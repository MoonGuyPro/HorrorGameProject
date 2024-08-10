using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControll : MonoBehaviour
{
    [SerializeField] private bool _bTestOpen;

    private Transform _rightDoor;
    private Transform _leftDoor;

    [SerializeField] private float _startRotationRight;
    [SerializeField] private float _startRotationLeft;

    [SerializeField] private float _endRotationRight;
    [SerializeField] private float _endRotationLeft;

    [SerializeField] private float _rotationDuration;

    private void Start()
    {
        _rightDoor = transform.GetChild(0);
        _leftDoor = transform.GetChild(1);
    }



    private void PlayAnimation(float rotationLeft, float rotationRight)
    {
        DOTween.Kill(transform);

        _rightDoor.DORotate(new Vector3(_rightDoor.eulerAngles.x, rotationRight,_rightDoor.eulerAngles.z), _rotationDuration);
        _leftDoor.DORotate(new Vector3(_rightDoor.eulerAngles.x, rotationLeft, _rightDoor.eulerAngles.z), _rotationDuration);
    }

    private void OnValidate()
    {
        if (_bTestOpen) {
            PlayAnimation(_endRotationLeft, _endRotationRight);
        }
        else
        {
            PlayAnimation(_startRotationLeft, _startRotationRight);
        }
        
    }
}
