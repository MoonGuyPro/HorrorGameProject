using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using System;
using System.ComponentModel;
using FMODUnity;

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
    [SerializeField] private EventReference scanLetterSound;
    private bool isScannerEquipped = false;
    private Transform lineEnd;
    public List<ScannableData> alreadyScanned;
    bool bDisplaying = false;
    Coroutine displayCoroutine;
    Coroutine displayLettersSoundsCoroutine;
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
                        displayCoroutine = StartCoroutine(DisplayPopupNoTweening(scanable.Data.DisplayName, scanable.Data.Description, 3.0f, 1.0f, scanable.transform.position));
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
        
        var timeBetweenLetters = fadeTime / name.Length;
        if (displayLettersSoundsCoroutine != null)
            StopCoroutine(displayLettersSoundsCoroutine);
        displayLettersSoundsCoroutine = StartCoroutine(LettersSounds(name.Length, timeBetweenLetters));
        
        DOTween.To(() => 0, x => popupName.text = name.Substring(0, x), name.Length, fadeTime).SetEase(Ease.Linear);
        
        popupName.color = Color.white;
        yield return new WaitForSeconds(fadeTime);

        lineRenderer.enabled = false;
        
        timeBetweenLetters = fadeTime / description.Length;
        if (displayLettersSoundsCoroutine != null)
            StopCoroutine(displayLettersSoundsCoroutine);
        Debug.Log("Fade time: " + fadeTime + " Description length: " + description.Length + " Time between letters: " + timeBetweenLetters);
        displayLettersSoundsCoroutine = StartCoroutine(LettersSounds(description.Length, timeBetweenLetters));
        
        DOTween.To(() => 0, x => popupDescription.text = description.Substring(0, x), description.Length, fadeTime).SetEase(Ease.Linear);
        
        popupDescription.color = Color.white;
        yield return new WaitForSeconds(fadeTime);

        yield return new WaitForSeconds(displayTime);

        popupName.DOColor(Color.clear, fadeTime);
        popupDescription.DOColor(Color.clear, fadeTime);
        yield return new WaitForSeconds(fadeTime);

        bDisplaying = false; 
    }
    
    IEnumerator DisplayPopupNoTweening(string name, string description, float displayTime, float fadeTime, Vector3 endPosition)
    {
        bDisplaying = true;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, lineStart.position);
        lineRenderer.material.color = Color.clear;
        lineRenderer.SetPosition(1, endPosition);

        popupName.text = "";
        popupDescription.text = "";
        popupName.color = Color.white;
        popupDescription.color = Color.white;
        
        var nameCount = name.Length;
        var descriptionCount = description.Length;

        var instance = RuntimeManager.CreateInstance(scanLetterSound);
        var soundLen = 0.07f;
        var speed = 0.0f;
        
        var timeBetweenLetters = fadeTime / name.Length;
        Debug.Log("name time: " + timeBetweenLetters);
        if (timeBetweenLetters < soundLen)
        {
            // adjusting the timing to fit the sound
            speed = soundLen / timeBetweenLetters - 1f;
            speed = Mathf.Clamp01(speed);
        }
        
        instance.setParameterByName("ScanLetterSpeed", speed);
        
        for (int i = 0; i < nameCount; i++)
        {
            popupName.text += name[i];
            instance.start();
            yield return new WaitForSeconds(timeBetweenLetters);
        }
        
        timeBetweenLetters = fadeTime / description.Length;
        if (timeBetweenLetters < soundLen)
        {
            // adjusting the timing to fit the sound
            speed = soundLen / timeBetweenLetters - 1f;
            speed = Mathf.Clamp01(speed);
        }
        
        instance.setParameterByName("ScanLetterSpeed", speed);
        
        for (int i = 0; i < descriptionCount; i++)
        {
            popupDescription.text += description[i];
            instance.start();
            yield return new WaitForSeconds(timeBetweenLetters);
        }
        
        yield return new WaitForSeconds(displayTime);
        popupName.color = Color.clear;
        popupDescription.color = Color.clear;
        lineRenderer.enabled = false;
        bDisplaying = false;
    }
        
    IEnumerator LettersSounds(int lettersCount, float timeBetweenLetters)
    {
        for (int i = 0; i < lettersCount; i++)
        {
            RuntimeManager.PlayOneShot(scanLetterSound);
            yield return new WaitForSeconds(timeBetweenLetters);
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
