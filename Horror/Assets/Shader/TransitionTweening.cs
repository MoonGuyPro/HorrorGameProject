using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TransitionTweening : MonoBehaviour
{
    PostProcessVolume volume;
    Bloom bloom;
    
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        // I'm testing here
        FadeOut();
    }

    void FadeOut()
    {
        bloom = volume.profile.GetSetting<Bloom>();
        
        DOTween.Sequence()
            .Append(DOTween.To(() => bloom.intensity, x => bloom.intensity.Override(x), 200f, 5f))
            /*.AppendInterval(1f)
            .Append(DOTween.To(() => volume.weight, x => volume.weight = x, 0f, 1f))*/
            .OnComplete(() =>
            {
                /*RuntimeUtilities.DestroyVolume(volume, true, true);
                Destroy(this);*/
            });    
    }
}
