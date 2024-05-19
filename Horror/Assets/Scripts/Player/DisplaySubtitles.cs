using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DisplaySubtitles : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI subtitles;

    public void Display(string text)
    {
        subtitles.text = text;
        DOTween.To(() => 0.0f, x => subtitles.color = new Color(1.0f, 1.0f, 1.0f, x), 10.0f, 10.0f)
            .OnComplete(() => subtitles.color = Color.clear);
    }
}
