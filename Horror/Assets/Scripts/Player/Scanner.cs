using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Data.Common;
using System.Security.Cryptography;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private TextMeshProUGUI subtitles;
    [SerializeField] private TextMeshProUGUI popupName;
    [SerializeField] private TextMeshProUGUI popupDescription;
    [SerializeField] private float maxDistance = 5;
    [SerializeField] private float radius = 0.45f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform lineStart;
    [SerializeField] private Animator scannerAnimator;
    private bool bDrawn = false;
    private Transform lineEnd;
    public List<ScannableData> alreadyScanned;
    bool bDisplaying = false;
    Coroutine displayCoroutine;
    Tweener displayTween;

    void Start() 
    {
        subtitles.color = Color.clear;
        popupName.color = Color.clear;
        popupDescription.color = Color.clear;
        lineRenderer.material.color = Color.clear;

        lineRenderer.SetPosition(0, lineStart.position);
        lineRenderer.enabled = false;

        if (alreadyScanned == null)
        {
            alreadyScanned = new List<ScannableData>();
        }
    }

    void Update()
    {
        if (bDisplaying)
            lineRenderer.SetPosition(0, lineStart.position);

        if (Input.GetButtonDown("DrawScanner"))
        {
            bDrawn =! bDrawn;
            scannerAnimator.SetBool("draw", bDrawn);
        }

        if (!Input.GetButtonDown("Scan") || !bDrawn) return;

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
                            popupName.DOKill();
                            popupDescription.DOKill();
                            StopCoroutine(displayCoroutine);
                        }
                        displayCoroutine = StartCoroutine(DisplayPopup(scanable.Data.DisplayName, scanable.Data.Description, 3.0f, 1.0f, scanable.transform.position));
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

    IEnumerator DisplayPopup(string name, string description, float displayTime, float fadeTime, Vector3 endPosition)
    {
        bDisplaying = true;
        lineRenderer.enabled = true;
        popupName.text = name;
        popupDescription.text = description;
        popupName.color = Color.white;
        popupDescription.color = Color.white;
        lineRenderer.material.color = Color.clear;
        lineRenderer.SetPosition(1, endPosition);
        yield return new WaitForSeconds(displayTime);
        //displayTween = subtitles.DOColor(Color.clear, fadeTime);
        //displayTween = DOTween.To(popupName.color, x => {popupName.color = x; popupDescription.color = x;}, Color.clear, fadeTime);
        popupName.DOColor(Color.clear, fadeTime);
        popupDescription.DOColor(Color.clear, fadeTime);
        //lineRenderer.DOColor(Color.white, Color.clear, fadeTime);
        //DOTween.To<Color>(lineRenderer.startColor, x => {lineRenderer.startColor = x; lineRenderer.endColor = x;}, Color.clear, fadeTime);
        lineRenderer.enabled = false; 
        yield return new WaitForSeconds(fadeTime);
        //lineRenderer.material.color = Color.clear;
        bDisplaying = false; 
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
