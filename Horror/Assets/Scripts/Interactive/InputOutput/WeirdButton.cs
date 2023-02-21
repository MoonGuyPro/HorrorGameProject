 using System;
 using FMOD.Studio;
 using FMODUnity;
 using UnityEngine;
 using UnityEngine.Serialization;
 using STOP_MODE = FMOD.Studio.STOP_MODE;

 public class WeirdButton : InputLogic
 {
     public Animator cubeAnim;

     [SerializeField] 
     private EventReference activationSound;
     
     [SerializeField] 
     private EventReference droneSound;

     private EventInstance droneInstance;

     private void Start()
     {
         droneInstance = RuntimeManager.CreateInstance(droneSound);
         droneInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.transform));
         droneInstance.start();
     }

     protected override void Behavior()
     {
         droneInstance.stop(STOP_MODE.ALLOWFADEOUT);
         RuntimeManager.PlayOneShotAttached(activationSound, gameObject);
         cubeAnim.SetTrigger("Activated");
     }
 }