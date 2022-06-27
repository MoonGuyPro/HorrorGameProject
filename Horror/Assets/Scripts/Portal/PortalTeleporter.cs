using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{

	public Transform player;
	public Transform reciever;
	public PortalTeleporter otherPortal;

	private int cooldown;

	private bool playerIsOverlapping = false;

	public GameObject soundSource;
	private AudioSource[] tpSounds;

    private void Start()
    {
		cooldown = 0;
		tpSounds = soundSource.GetComponents<AudioSource>();
		foreach (AudioSource tpSound in tpSounds)
        {
			tpSound.volume = 0.3f;
        }
	}

    // Update is called once per frame
    void Update()
	{
		if (cooldown == 0)
        {
			if (playerIsOverlapping)
			{
				Vector3 portalToPlayer = player.position - transform.position;
				float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

				// If this is true: The player has moved across the portal
				if (dotProduct < 0f)
				{
					tpSounds[Random.Range(0, 3)].Play();
					// Teleport him!
					float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
					rotationDiff += 180;
					player.Rotate(Vector3.up, rotationDiff);

					Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
					player.position = reciever.position + positionOffset;

					playerIsOverlapping = false;
					
					// setting a teleporting cooldown on both portals
					setCooldown();
					otherPortal.setCooldown();
				}
			}
		} else
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
		// this value should be tied to frame rate, but isn't.
		// setting this to 60 or 120 does not mean a second or two
		cooldown = 500;
    }
}
