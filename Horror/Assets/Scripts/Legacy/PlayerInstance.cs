using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
	public static PlayerInstance instance;
	public static Transform playerCamera;
	
    void Start()
    {
        if (instance != null) Destroy(gameObject);
		else instance = this;
        DontDestroyOnLoad(gameObject);
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
