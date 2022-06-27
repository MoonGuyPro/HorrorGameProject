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
    public float speedX;
    public float speedY;
    public float speedZ;
    private Vector3 pos;
    private Vector3 start;
    private bool isMoving = false;

    private AudioSource creature_rawr;
    private bool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        start = creature.transform.position;
        X = start.x;
        Y = start.y;
        Z = start.z;
        creature_rawr = creature.GetComponent<AudioSource>();
        creature_rawr.volume = 0.5f;

        X = X / 1;
        Y = Y / 1;
        Z = Z / 1;
        Debug.Log(X.ToString());
        Debug.Log(Y.ToString());
        Debug.Log(Z.ToString());

        isPlaying = false;
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
            X = X + speedX;
            Y = Y + speedY;
            Z = Z + speedZ;

            pos = new Vector3(X, Y, Z);
            creature.transform.position = pos;

            if (!isPlaying)
            {
                creature_rawr.Play();
                isPlaying = true;
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
