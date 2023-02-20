using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VineTrap : MonoBehaviour
{
    private FPSController fpsController;
    private Animator anim;
    private float playerHeight = 2.0f;
    
    [SerializeField]
    private EventReference eventRef;

    // Save player rotation on level change to make it seamless
    private static Vector3 lastPlayerRotation;
    private static Vector3 lastCameraRotation;
    [SerializeField] private bool trapOnSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        fpsController = Camera.main.gameObject.GetComponentInParent<FPSController>();
        anim = GetComponentInChildren<Animator>();
        
        // Play release animation if VineTrap changed level
        if (trapOnSpawn)
        {
            trapOnSpawn = false;
            IEnumerator coEnumerator = AfterChangeLevel();
            StartCoroutine(coEnumerator);
        }
    }
    
    // Called by VineTrigger - trap teleports to other location
    public void TrapAndTeleport(Transform targetLocation)
    {
        IEnumerator coEnumerator = Teleport(targetLocation);
        StartCoroutine(coEnumerator);
    }

    // Called by VineTrigger - trap teleports to other level
    public void TrapAndChangeLevel(string level)
    {
        IEnumerator coEnumerator = ChangeLevel(level);
        StartCoroutine(coEnumerator);
    }
    
    // Teleport implementation
    private IEnumerator Teleport(Transform targetLocation)
    {
        // Stuck player and teleport vines to him
        fpsController.bStuck = true;
        transform.position = fpsController.transform.position + new Vector3(0, -playerHeight, 0);
        
        // Play closing animation
        anim.SetBool("Opened", false);
        RuntimeManager.PlayOneShotAttached(eventRef, gameObject);
        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Teleport
        transform.position = targetLocation.transform.position;
        fpsController.transform.position = targetLocation.position + new Vector3(0, playerHeight, 0);
        
        // Rotate camera and vines
        fpsController.yaw = targetLocation.eulerAngles.y + 180.0f;
        transform.eulerAngles = targetLocation.eulerAngles;

        // Play opening animation
        anim.SetBool("Opened", true);
        RuntimeManager.PlayOneShotAttached(eventRef, gameObject);
        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        
        // Unstuck player
        fpsController.bStuck = false;
    }

    // Level change implementation
    private IEnumerator ChangeLevel(string level)
    {
        // Stuck player and teleport vines to him
        fpsController.bStuck = true;
        transform.position = fpsController.transform.position + new Vector3(0, -playerHeight, 0);
        
        // Play closing animation
        anim.SetBool("Opened", false);
        RuntimeManager.PlayOneShotAttached(eventRef, gameObject);
        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        
        // Cache rotation and change level
        lastPlayerRotation = fpsController.transform.eulerAngles;
        lastCameraRotation = Camera.main.transform.localEulerAngles;
        //print(Camera.main.transform.localEulerAngles);
        trapOnSpawn = true;
        SceneManager.LoadScene(level);
    }
    
    // Level change implementation part 2: Electric Boogaloo
    private IEnumerator AfterChangeLevel()
    {
        // Stuck player and teleport vines to him
        fpsController.bStuck = true;
        transform.position = fpsController.transform.position + new Vector3(0, -playerHeight, 0);

        //FIXME:
        //  If player looked up, then... make him still look up
        //  This line of code makes him at least look forward and not at the ground after spawning
        //      - Krystian
        lastCameraRotation.x = lastCameraRotation.x > 90.0f ? 0.0f : lastCameraRotation.x;
        
        // Restore camera rotation
        transform.eulerAngles = lastPlayerRotation;
        Camera.main.transform.localEulerAngles = lastCameraRotation;

        // Play opening animation
        anim.SetBool("Opened", true);
        RuntimeManager.PlayOneShotAttached(eventRef, gameObject);
        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        
        //print(lastCameraRotation);
        //print(Camera.main.transform.localEulerAngles);
        
        // Unstuck player
        fpsController.bStuck = false;
    }
}
