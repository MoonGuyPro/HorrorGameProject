using UnityEngine;

public class Skybox : MonoBehaviour
{
    public float scale = 1.0f;
    public Camera skyboxCamera;
    private Vector3 offset;

    void Start()
    {
        offset = PlayerInstance.GetCameraPosition() - transform.position;
    }
    
    void Update()
    {
        skyboxCamera.transform.position = (PlayerInstance.GetCameraPosition() - offset) * scale;
        skyboxCamera.transform.rotation = PlayerInstance.GetCameraRotation();
    }
}
