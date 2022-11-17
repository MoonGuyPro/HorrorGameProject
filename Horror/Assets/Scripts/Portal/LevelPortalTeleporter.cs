using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPortalTeleporter : MonoBehaviour
{
	public string levelName;
/*	public Transform player;
	public Vector3 positionAfterTeleporting;
	public GameObject soundSource;
	public bool canTeleport;
	public bool playSound;

	private AudioSource[] tpSounds;*/

	// Krystian here - We don't need all of these variables rn I guess

	void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") 
		{
			//tpSounds[Random.Range(0, 3)].Play();
			SceneManager.LoadScene(levelName);
			//player.position = positionAfterTeleporting;
		}
    }
	
	// Kris here - I am not adding cooldown since for now the portal is one-way only. 
	// That's also why I am not inheriting anything from PortalTeleporter.
	// If it ever changes, I will make it consistent with regular portals.

}