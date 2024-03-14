using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using System;
using System.ComponentModel;

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
    private bool isScannerEquipped = false;
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

    void OnEnable ()
    {
		InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
		InputActionMap inputActionMap = inputActionAsset.FindActionMap("Player");
        inputActionMap.FindAction("Scan").performed += Scan;
        inputActionMap.FindAction("Equip").performed += EquipScanner;
    }

    void OnDisable ()
    {
        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
		InputActionMap inputActionMap = inputActionAsset.FindActionMap("Player");
        inputActionMap.FindAction("Scan").performed -= Scan;
        inputActionMap.FindAction("Equip").performed -= EquipScanner;
    }

    void EquipScanner(InputAction.CallbackContext context)
    {
        isScannerEquipped =! isScannerEquipped;
        scannerAnimator.SetBool("draw", isScannerEquipped);
    }

    void Scan(InputAction.CallbackContext context)
    {
        if (!isScannerEquipped)
            return;

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
                            DOTween.KillAll();
                            StopCoroutine(displayCoroutine);
                        }
                        displayCoroutine = StartCoroutine(DisplayPopup(scanable.Data.DisplayName, scanable.Data.Description, 3.0f, 1.0f, scanable.transform.position));
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

    void Update()
    {
        if (bDisplaying)
            lineRenderer.SetPosition(0, lineStart.position);
    }

    IEnumerator DisplayPopup(string name, string description, float displayTime, float fadeTime, Vector3 endPosition)
    {
        bDisplaying = true;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, lineStart.position);
        lineRenderer.material.color = Color.clear;
        lineRenderer.SetPosition(1, endPosition);

        popupName.text = "";
        popupDescription.text = "";

        DOTween.To(() => 0, x => popupName.text = name.Substring(0, x), name.Length, fadeTime);
        popupName.color = Color.white;
        yield return new WaitForSeconds(fadeTime);

        lineRenderer.enabled = false;
        DOTween.To(() => 0, x => popupDescription.text = description.Substring(0, x), description.Length, fadeTime);
        popupDescription.color = Color.white;
        yield return new WaitForSeconds(fadeTime);

        yield return new WaitForSeconds(displayTime);

        popupName.DOColor(Color.clear, fadeTime);
        popupDescription.DOColor(Color.clear, fadeTime);
        yield return new WaitForSeconds(fadeTime);

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
