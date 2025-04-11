using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneExit : MonoBehaviour
{
	public string targetSceneName;
	//public string exitName;

	public float transitionTime = 1.0f;
	
	public GameObject playerTransition;

	private TransitionController transitionController;

	void Awake()
	{
		transitionController = playerTransition.GetComponent<TransitionController>();
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player")) // Only player can use SceneExits
		{ 
			//PlayerPrefs.SetString("LastExitName", exitName);
			StartCoroutine(LoadLevel(targetSceneName));
		}
	}

	IEnumerator LoadLevel(string levelName)
	{
		transitionController.EndLevel();

		yield return new WaitForSeconds(transitionTime);

		SceneManager.LoadScene(levelName);
	}
}
