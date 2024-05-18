using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneExit : MonoBehaviour
{
	public string targetSceneName;
	//public string exitName;

	public float transitionTime = 1.0f;
	
	[ColorUsage(false)] public Color transitionColor = Color.white;

	public GameObject playerTransition;

	private Animator transitionAnim;
	private RawImage rawImage;

	void Awake()
	{
		transitionAnim = playerTransition.GetComponent<Animator>();
		rawImage = playerTransition.GetComponent<RawImage>();
		rawImage.color = transitionColor;
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
		transitionAnim.SetTrigger("Start");

		yield return new WaitForSeconds(transitionTime);

		SceneManager.LoadScene(levelName);
	}
}
