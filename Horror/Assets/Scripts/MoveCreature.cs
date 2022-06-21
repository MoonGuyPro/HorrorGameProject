using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCreature : MonoBehaviour
{
    public GameObject creature;
    private float X;
    private float Y;
    private float Z;
    public float endX;
    public float endY;
    public float endZ;
    public float speed;
    private Vector3 start;
    private Vector3 pos;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        start = creature.transform.position;
        X = start.x;
        Y = start.y;
        Z = start.z;
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            isMoving = true;
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            Debug.Log(Z);
            Z = Z - speed;
            pos = new Vector3(X, Y, Z);
            creature.transform.position = pos;
            if(Z <= endZ)
            {
                Z = endZ;
            }
        }
        if(X == endX && Y == endY && Z == endZ)
        {
            isMoving = false;
            Destroy(creature);
            Destroy(gameObject);
        }
    }

}
