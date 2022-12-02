using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
	public static PlayerInstance instance;
	public static Transform playerCamera;
	
	// Krystian here -	I disabled singleton (inventory isn't kept between levels)
	//					cuz I have no idea how to fix it rn xd
    void Start()
    {
        /*if (instance != null) Destroy(gameObject);
		else instance = this;
        DontDestroyOnLoad(gameObject);*/
        instance = this;
		playerCamera = GetComponent<PlayerInteraction>().PlayerCamera;	
    }

    public static Vector3 GetCameraPosition()
    {
	    return playerCamera.transform.position;
    }

    public static Quaternion GetCameraRotation()
    {
	    return playerCamera.transform.rotation;
    }
}
