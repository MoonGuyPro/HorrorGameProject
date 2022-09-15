using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPortalTeleporter : MonoBehaviour
{
	public Transform player;
	public string levelName;
	//public Vector3 positionAfterTeleporting;
	public GameObject soundSource;
	public bool canTeleport;
	public bool playSound;

	private AudioSource[] tpSounds;

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