using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : OutputLogic
{
	public GameObject player;
	public Vector3 target;
	public float speed = 10.0f;
	public bool isSticky = true; // Should the player move with the platform when it's moving?
	public bool oneWay = false; // If set to true, the platform will not return to initial position once it reaches target position
	//public float returnDelay = 0.5f;
	
	private Vector3 initialPosition;
	private bool isMoving = false;
	private bool isReturning = false;
	
	new void Start() // "overrides" Start() from OutputLogic to save initial position first
	{
		initialPosition = transform.position;
		base.Start();
	}
	
	void Update()
	{
		if (isMoving) 
		{			
			if (!isReturning)
			{
				transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
				if (!oneWay)
				{
					isReturning = transform.position == target;
				}
			}
			else // Makes the platform return if not set to oneWay
			{
				transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
				isReturning = transform.position != initialPosition;
			}
		}
	}

    protected override void Behavior()
    {
        isMoving = !isMoving;
    }
	
	// The following functions make the player "stick" to the platform if the platform is sticky
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == player && isSticky)
		{
			player.transform.parent = transform;
		}
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player && isSticky)
		{
			player.transform.parent = null;
		}
	}
	
}
