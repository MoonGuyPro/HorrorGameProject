using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntrance : MonoBehaviour
{
	public string lastExitName;
	public bool overrideRotation = false;
	public float xRotation, yRotation, zRotation;
	
	Camera tempCamera;
	MainCamera mainCam;
	
    // Start is called before the first frame update
    void Start()
    {
		//Debug.LogWarning("Gowno.");
		//GameObject[] temp = GameObject.FindGameObjectsWithTag("MainCamera");
		//mainCamera = temp[0].GetComponentInChildren<MainCamera>();
		tempCamera = Camera.main;
		//Debug.LogWarning("Kurwa.");
		mainCam = tempCamera.GetComponent<MainCamera>();
		if (mainCam) 
		{
			//Debug.LogWarning("Kupa.");
			mainCam.FindPortals();
		}
		else
		{
			//Debug.LogWarning("Dupa.");
		}
		//Debug.LogWarning("Chuj.");

        if (PlayerPrefs.GetString("LastExitName") == lastExitName)
		{
			PlayerInstance.instance.transform.position = transform.position;
			if (overrideRotation) 
			{
				PlayerInstance.instance.transform.rotation = Quaternion.Euler(new Vector3(xRotation, yRotation, zRotation));
			}
			else 
			{
				PlayerInstance.instance.transform.eulerAngles = transform.eulerAngles;
			}
		}
    }
}
