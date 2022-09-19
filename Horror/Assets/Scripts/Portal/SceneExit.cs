using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneExit : MonoBehaviour
{
	public string targetScene;
	public string exitName;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player")) { // Only players can use SceneExit portals
			PlayerPrefs.SetString("LastExitName", exitName);
			SceneManager.LoadScene(targetScene);
		}
	}
}
