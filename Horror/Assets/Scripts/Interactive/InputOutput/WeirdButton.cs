 using System;
 using FMOD.Studio;
 using FMODUnity;
 using UnityEngine;
 using UnityEngine.Serialization;
 using STOP_MODE = FMOD.Studio.STOP_MODE;

 public class WeirdButton : MonoBehaviour
 {
     public Animator cubeAnim;

     [SerializeField] 
     private EventReference activationSound;
     
     [SerializeField] 
     private EventReference droneSound;

     private EventInstance droneInstance;

     private void Start()
     {
         // Don't make microwave sounds if there is no cube
         if (cubeAnim.gameObject.activeSelf)
         {
             droneInstance = RuntimeManager.CreateInstance(droneSound);
             droneInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.transform));
             droneInstance.start();
         }
     }

    public void OnClick()
    {
         cubeAnim.gameObject.SetActive(true);
         droneInstance.stop(STOP_MODE.ALLOWFADEOUT);
         RuntimeManager.PlayOneShotAttached(activationSound, gameObject);
         cubeAnim.SetTrigger("Activated");
    }

 }