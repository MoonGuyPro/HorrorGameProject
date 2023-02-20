 using UnityEngine;
 using UnityEngine.Serialization;

 public class WeirdButton : InputLogic
 {
     public Animator cubeAnim;
     
     protected override void Behavior()
     {
         cubeAnim.SetTrigger("Activated");
     }
 }