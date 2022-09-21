using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntrance : MonoBehaviour
{
	public string lastExitName;
	public bool overrideRotation = false;
	public float xRotation, yRotation, zRotation;
	
    // Start is called before the first frame update
    void Start()
    {
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
