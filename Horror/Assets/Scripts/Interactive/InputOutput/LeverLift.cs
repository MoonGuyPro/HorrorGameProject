using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverLift : MonoBehaviour
{
    public Animator leverAnim;
    public Animator liftAnim;
    public LiftValue liftValue;
    //[SerializeField]
    //private EventReference activationSound;

    private EventInstance droneInstance;
    bool leverState;

    private void Start()
    {
        leverAnim.gameObject.SetActive(true);
        // Don't make microwave sounds if there is no cube
        if (leverAnim.gameObject.activeSelf)
        {
            //droneInstance = RuntimeManager.CreateInstance(droneSound);
            droneInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.transform));
            droneInstance.start();
        }
        liftAnim.ResetTrigger("active");
        leverAnim.ResetTrigger("active");
    }

    public void OnClick()
    {
        //droneInstance.stop(STOP_MODE.ALLOWFADEOUT);
        //RuntimeManager.PlayOneShotAttached(activationSound, gameObject);
        if (!liftValue.isUp)
        {
            leverAnim.SetTrigger("active");
            liftAnim.SetTrigger("active");
        }
        else
        {
            leverAnim.ResetTrigger("active");
            liftAnim.ResetTrigger("active");
        }

        liftValue.isUp = !liftValue.isUp;
    }

}