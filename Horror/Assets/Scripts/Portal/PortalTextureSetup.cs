using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{

	public Camera cameraRoom2;
	public Camera cameraRoom3;

	public Material cameraMatA;
	public Material cameraMatB;

	// Use this for initialization
	void Start()
	{
		if (cameraRoom2.targetTexture != null)
		{
			cameraRoom2.targetTexture.Release();
		}
		cameraRoom2.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMatA.mainTexture = cameraRoom2.targetTexture;

		if (cameraRoom3.targetTexture != null)
		{
			cameraRoom3.targetTexture.Release();
		}
		cameraRoom3.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMatB.mainTexture = cameraRoom3.targetTexture;
	}

}
