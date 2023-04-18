using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHead : MonoBehaviour
{

    public Transform target;
    [SerializeField]
    private int speed = 5;

    // Update is called once per frame
    void OnTriggerStay(Collider Other)
    {
        if (Other.gameObject.tag == "Player" && target != null)
        {
            Vector3 relativePos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            Quaternion current = transform.rotation;

            transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime
                * speed);


        }
    }
}
