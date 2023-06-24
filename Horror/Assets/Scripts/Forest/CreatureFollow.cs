using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureFollow : MonoBehaviour
{

    public Transform player;
    public float lerp = 0.1f;

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3(transform.position.x, transform.position.y, player.position.z);
        transform.position = Vector3.Lerp(transform.position, target, lerp);
    }
}
