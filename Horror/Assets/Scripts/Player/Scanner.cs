using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Data.Common;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;

    [SerializeField] private TextMeshProUGUI subtitles;
    [SerializeField] private float maxDistance = 5;
    [SerializeField] private float radius = 0.45f;

    bool bDisplaying = false;
    Coroutine displayCoroutine;
    Tweener displayTween;

    void Start() 
    {
        subtitles.color = Color.clear;
    }

    void Update()
    {
        if (!Input.GetButtonDown("Scan")) return;

        RaycastHit hit;
        if (Physics.SphereCast(playerCamera.position, radius, playerCamera.forward, out hit, maxDistance))
        {
            if (hit.transform.CompareTag("Scannable"))
            {
                Scannable scanable = hit.transform.GetComponentInParent<Scannable>();

                if (scanable is not null)
                {
                    if (scanable.Data is not null)
                    {
                        if (bDisplaying)
                        {
                            //displayTween.Kill();
                            //DOTween.Kill(subtitles);
                            Debug.Log(subtitles.DOKill());
                            StopCoroutine(displayCoroutine);
                        }
                        displayCoroutine = StartCoroutine(DisplaySubtitles(scanable.Data.Description, 3.0f, 3.0f));
                        Debug.Log(scanable.Data.Description);
                    }
                    else
                    {
                        Debug.LogWarning("Scanable has no data!");
                    }
                }
                else
                {
                    Debug.LogWarning("Scanable component is missing!");
                }
            }
        } 
    }

    IEnumerator DisplaySubtitles(string text, float displayTime, float fadeTime)
    {
        bDisplaying = true;
        subtitles.text = text;
        subtitles.color = Color.white;
        yield return new WaitForSeconds(displayTime);
        displayTween = subtitles.DOColor(Color.clear, fadeTime);
        bDisplaying = false;
    }
}
