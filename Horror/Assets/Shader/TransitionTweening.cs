using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TransitionTweening : MonoBehaviour
{
    [SerializeField] private bool fadeinOnStart = false;
    [SerializeField] private float maxIntensity = 250f;
    public float fadeoutTime = 5f;
    public float fadeinTime = 5f;
    
    PostProcessVolume volume;
    Bloom bloom;
    
    void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
        if (fadeinOnStart)
        {
            FadeIn();
        }
    }

    // Fadeout effect after picking up Supercube
    public void FadeOut()
    {
        bloom = volume.profile.GetSetting<Bloom>();
        DOTween.Sequence()
            .Append(DOTween.To(() => bloom.intensity, x => bloom.intensity.Override(x), maxIntensity, fadeoutTime))
            .OnComplete(() =>
            {
                
            });    
    }
    
    // Fadein effect after Supercube teleports player to next level
    public void FadeIn()
    {
        bloom = volume.profile.GetSetting<Bloom>();
        bloom.intensity.Override(maxIntensity);
        DOTween.Sequence()
            .Append(DOTween.To(() => bloom.intensity, x => bloom.intensity.Override(x), 0f, fadeinTime))
            .OnComplete(() =>
            {

            });    
    }
}
