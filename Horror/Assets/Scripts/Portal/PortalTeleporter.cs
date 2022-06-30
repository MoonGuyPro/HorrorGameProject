using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{

	public Transform player;
	//Portal that this portal leads to
	public GameObject otherPortal;

	private Transform otherTransform;
	private PortalTeleporter otherPortalTeleporter;

	private int cooldown;

	private bool playerIsOverlapping = false;

	public GameObject soundSource;
	private AudioSource[] tpSounds;

	public bool canTeleport;

	private void Start()
	{
		otherTransform = otherPortal.transform;
		otherPortalTeleporter = otherPortal.GetComponent<PortalTeleporter>();
		cooldown = 0;
		tpSounds = soundSource.GetComponents<AudioSource>();
		for (int i = 0; i < tpSounds.Length; i++)
		{
			tpSounds[i].volume = 0.09f;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (cooldown == 0)
		{
			if (playerIsOverlapping && canTeleport)
			{
				Vector3 portalToPlayer = player.position - transform.position;
				float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

				// If this is true: The player has moved across the portal
				if (dotProduct < 0f)
				{
					tpSounds[Random.Range(0, 3)].Play();
					// Teleport him!
					float rotationDiff = -Quaternion.Angle(transform.rotation, otherTransform.rotation);
					rotationDiff += 180;
					player.Rotate(Vector3.up, rotationDiff);

					Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
					player.position = otherTransform.position + positionOffset;

					playerIsOverlapping = false;

					// setting a teleporting cooldown on both portals
					setCooldown();
					otherPortalTeleporter.setCooldown();
				}
			}
		}
		else
		{
			cooldown--;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = false;
		}
	}

	public void setCooldown()
	{
		// this value is tied to frame rate BUT THE FRAME RATE IS NOT CAPPED IN PLAY MODE
		// ONLY IN BUILD THIS ACTUALLY TAKES EFFECT
		// meaning in play mode 750 is about a second, but in build its 750/60 = 12.5 seconds
		// changing this before building is necessary not to make portal awkward
		cooldown = 750;
	}
}