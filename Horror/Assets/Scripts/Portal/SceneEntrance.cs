using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntrance : MonoBehaviour
{
	public string lastExitName;
	
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("LastExitName") == lastExitName)
		{
			PlayerInstance.instance.transform.position = transform.position;
			PlayerInstance.instance.transform.eulerAngles = transform.eulerAngles;
		}
    }
}
