using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTrigger : MonoBehaviour
{
    private float _offsetY;
    private Transform _playerTransform;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Heloo");
            _playerTransform = other.gameObject.transform;
            _offsetY = other.gameObject.transform.position.y - transform.position.y;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("A"+_playerTransform.position);
            _playerTransform.position = new Vector3(_playerTransform.position.x, _offsetY + transform.position.y, _playerTransform.position.z);
            Debug.Log("B"+_playerTransform.position);
        }
    }
}
