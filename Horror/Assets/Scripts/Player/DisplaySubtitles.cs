using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySubtitles : MonoBehaviour
{
    [SerializeField] 
	private TextMeshProUGUI subtitles;
    [SerializeField] 
	private Image panel;
	[SerializeField] 
	private GameObject crosshair;

    private readonly float displayDuration = 10.0f;

    public void Display(string text)
    {
        subtitles.text = text;
        DOTween.To(() => 0.0f, x => {
            subtitles.color = new Color(1.0f, 1.0f, 1.0f, x); 
            panel.color = new Color(0.0f, 0.0f, 0.0f, Mathf.Min(x, 0.77f));
            }, 10.0f, displayDuration)
            .OnComplete(() => {subtitles.color = Color.clear; panel.color = Color.clear; });
    }
	
	public void ToggleCrosshair(bool active)
	{
		crosshair.SetActive(active);
	}
}
