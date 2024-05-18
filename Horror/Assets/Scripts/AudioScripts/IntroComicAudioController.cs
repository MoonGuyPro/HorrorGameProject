using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class IntroComicAudioController : MonoBehaviour
{
    [SerializeField] private EventReference music;
    [SerializeField] private EventReference stoneExplosion;
    private EventInstance instance;

    private void Start()
    {
        instance = RuntimeManager.CreateInstance(music);
        instance.start();
    }

    public void OnAddBass()
    {
        instance.setParameterByName("ComicBass", 1f);
    }

    public void OnAddDrums()
    {
        instance.setParameterByName("ComicDrums", 1f);
    }

    public void PlayStoneExplosion()
    {
        RuntimeManager.PlayOneShot(stoneExplosion);
    }

    private void OnDestroy()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }
}
