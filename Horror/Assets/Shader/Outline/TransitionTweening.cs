using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

public class TransitionTweening : MonoBehaviour
{
    [SerializeField] private bool fadeinOnStart = false;
    [SerializeField] private float fadeStrength = 2f;
    public float fadeoutTime = 5f;
    public float fadeinTime = 5f;
    
    PostProcessVolume volume;
    PostProcessOutline outline;
    
    void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
        outline = volume.profile.GetSetting<PostProcessOutline>();
        if (fadeinOnStart)
        {
            FadeIn();
        }
    }

    // Fadeout effect after picking up Supercube
    public void FadeOut()
    {
        DOTween.Sequence()
            .Append(DOTween.To(() => outline.lowCutOff, x => outline.lowCutOff.Override(x), fadeStrength, fadeoutTime))
            .OnComplete(() =>
            {
                
            });    
    }
    
    // Fadein effect after Supercube teleports player to next level
    public void FadeIn()
    {
        outline.lowCutOff.Override(fadeStrength);
        DOTween.Sequence()
            .Append(DOTween.To(() => outline.lowCutOff, x => outline.lowCutOff.Override(x), 0f, fadeinTime))
            .OnComplete(() =>
            {

            });    
    }
}
