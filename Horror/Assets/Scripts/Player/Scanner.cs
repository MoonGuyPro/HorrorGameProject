using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using FMODUnity;
using Unity.VisualScripting;

[Serializable]
class AnimParams
{
    public Animator scannerAnimator; 
    public Animator cubeAnimator;
    public MeshRenderer cubeRenderer;
    public MeshRenderer screenRenderer;
    public float cubeAcceleration = 3.0f;
    public float minCubeSpeed = 1.0f, maxCubeSpeed = 20.0f;
    public AnimColors color;
    [Serializable] public class AnimColors
    {
        public Color normal = Color.cyan, invalid = Color.red, hover = Color.yellow, scanning = Color.green;
    } 
    public AnimTimes time;
    [Serializable] public class AnimTimes
    {
        public float wrong = 0.3f, scanning = 2.0f, nameLetterScale = 1.0f, descriptionLetterScale = 0.25f;
    } 
    public AnimTextures textures;
    [Serializable] public class AnimTextures
    {
        public Texture2D normal, invalid, hover;
        public Texture2D[] scanning;
    }

}

[Serializable]
class InputParams
{
    public float scanTime = 2.0f;
}
public class Scanner : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private TextMeshProUGUI subtitles;
    [SerializeField] private TextMeshProUGUI popupName;
    [SerializeField] private TextMeshProUGUI popupDescription;
    [SerializeField] private float maxDistance = 5;
    [SerializeField] private float radius = 0.05f;
    [SerializeField] private float scanCooldown = 0.33f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform lineStart;
    [SerializeField] private EventReference scanLetterSound;
    [SerializeField] private EventReference scannerDrawSound;
    [SerializeField] private EventReference scannerHideSound;
    [SerializeField] private EventReference noScanTargetSound;
    [SerializeField] private AnimParams animParams;
    [SerializeField] private InputParams inputParams;
    
    private bool isScannerEquipped = false;
    private bool canScan = true;
    private Transform lineEnd;
    public List<ScannableData> alreadyScanned;
    bool bDisplaying = false;
    Coroutine displayCoroutine;
    Tween displayTween;
    Scannable scannable;
    Color currentScannerColor; // Normal or hover
    Texture2D currentScreenTexture;
    Coroutine scanCooldownCoroutine;
    bool isScanning = false;
    float scanningProgress = 0.0f;

    void Start() 
    {
        subtitles.color = Color.clear;
        popupName.color = Color.clear;
        popupDescription.color = Color.clear;
        lineRenderer.material.color = Color.clear;

        lineRenderer.SetPosition(0, lineStart.position);
        lineRenderer.enabled = false;

        currentScannerColor = animParams.color.normal;

        if (alreadyScanned == null)
        {
            alreadyScanned = new List<ScannableData>();
        }
    }

    void OnEnable ()
    {
		InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
		InputActionMap inputActionMap = inputActionAsset.FindActionMap("Player");
        // scanAction = inputActionMap.FindAction("Scan");
        inputActionMap.FindAction("Scan").performed += OnScanPressed;
        inputActionMap.FindAction("Scan").canceled += OnScanReleased;
        inputActionMap.FindAction("Equip").performed += EquipScanner;
    }

    void OnDisable ()
    {
        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
		InputActionMap inputActionMap = inputActionAsset.FindActionMap("Player");
        inputActionMap.FindAction("Scan").performed -= OnScanPressed;
        inputActionMap.FindAction("Scan").canceled -= OnScanReleased;
        inputActionMap.FindAction("Equip").performed -= EquipScanner;
    }

    void EquipScanner(InputAction.CallbackContext context)
    {
        isScannerEquipped =! isScannerEquipped;
        animParams.scannerAnimator.SetBool("draw", isScannerEquipped);
        StartCoroutine(ScannerSoundWithDelay(isScannerEquipped));
    }

    void OnScanPressed(InputAction.CallbackContext context)
    {
        if (!isScannerEquipped || !canScan)
            return;

        if (scannable == null)
        {
            if (!DOTween.IsTweening(animParams.cubeAnimator))
            {
                animOnInvalid();
                RuntimeManager.PlayOneShot(noScanTargetSound);
                scanCooldownCoroutine = StartCoroutine(ScanCooldown(animParams.time.wrong));
            }
        }
        else
        {
            isScanning = true;
        }
    }

    void OnScanReleased(InputAction.CallbackContext context)
    {
        isScanning = false;
    }

    void OnScanned()
    {
        if (!DOTween.IsTweening(animParams.cubeAnimator))
        {
            animOnScan();

            if (bDisplaying)
                StopCoroutine(displayCoroutine);

            displayCoroutine = StartCoroutine(
                DisplayPopupNoTweening(scannable.Data.DisplayName, scannable.Data.Description, 1.0f, scannable.transform.position));

            scanCooldownCoroutine = StartCoroutine(ScanCooldown(animParams.time.scanning));
        }
    }

    void Update()
    {
        if (!isScannerEquipped)
            return;

        RaycastHit hit;
        if (Physics.SphereCast(playerCamera.position, radius, playerCamera.forward, out hit, maxDistance))
        {
            if (hit.transform.CompareTag("Scannable"))
            {
                scannable = hit.transform.GetComponentInParent<Scannable>();
            }
        }
        else
        {
            scannable = null;
            currentScannerColor = animParams.color.normal;
            currentScreenTexture = animParams.textures.normal;
            scanningProgress = 0.0f;
        }

        if (canScan && isScanning)
        {
            scanningProgress += Time.deltaTime / inputParams.scanTime;
        }
        else
        {
            scanningProgress = 0.0f;
        }

        if (isScanning)
        {
            int frames = animParams.textures.scanning.Length;
            currentScreenTexture = animParams.textures.scanning[(int)Math.Min(Math.Floor(scanningProgress * (frames + 1)), frames - 1)];
        } 
        else if (scannable != null)
        {
            currentScannerColor = animParams.color.hover;
            currentScreenTexture = animParams.textures.hover;
        }
        else
        {
            currentScannerColor = animParams.color.normal;
            currentScreenTexture = animParams.textures.normal;
        }
        
        if (scanningProgress >= 1.0f)
        {
            OnScanned();
        }

        if (!DOTween.IsTweening(animParams.cubeAnimator))
        {
            animParams.cubeRenderer.material.SetColor("_EmissionColor", currentScannerColor);
            animParams.screenRenderer.material.SetColor("_EmissionColor", currentScannerColor);
            animParams.screenRenderer.material.SetTexture("_EmissionMap", currentScreenTexture);
            animParams.cubeAnimator.speed = Mathf.Lerp(animParams.minCubeSpeed, animParams.maxCubeSpeed, scanningProgress);
        }

        if (bDisplaying)
            lineRenderer.SetPosition(0, lineStart.position);
    }
    
    IEnumerator DisplayPopupNoTweening(string name, string description, float fadeTime, Vector3 endPosition)
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
        
        // var timeBetweenLetters = fadeTime / name.Length;

        // if (timeBetweenLetters < soundLen)
        // {
        //     // adjusting the timing to fit the sound
        //     speed = soundLen / timeBetweenLetters - 1f;
        //     speed = Mathf.Clamp01(speed);
        // }
        
        speed = soundLen * animParams.time.nameLetterScale;
        instance.setParameterByName("ScanLetterSpeed", speed);
        
        for (int i = 0; i < nameCount; i++)
        {
            popupName.text += name[i];
            instance.start();
            yield return new WaitForSeconds(speed);
        }
        
        // timeBetweenLetters = fadeTime / description.Length;
        // if (timeBetweenLetters < soundLen)
        // {
        //     // adjusting the timing to fit the sound
        //     speed = soundLen / timeBetweenLetters - 1f;
        //     speed = Mathf.Clamp01(speed);
        // }
        
        speed = soundLen * animParams.time.descriptionLetterScale;
        instance.setParameterByName("ScanLetterSpeed", speed);
        
        for (int i = 0; i < descriptionCount; i++)
        {
            popupDescription.text += description[i];
            instance.start();
            yield return new WaitForSeconds(speed);
        }
        
        yield return new WaitForSeconds(fadeTime);
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
    
    IEnumerator ScannerSoundWithDelay(bool equipped)
    {
        if (equipped)
        {
            yield return new WaitForSeconds(0.1f);
            RuntimeManager.PlayOneShot(scannerDrawSound);
        }
        else
        {
            yield return new WaitForSeconds(0.35f);
            RuntimeManager.PlayOneShot(scannerHideSound);
        }
    }

    void animOnInvalid()
    {
        // if (DOTween.IsTweening(anim.cubeAnimator))
        //     return;

        DOTween.To(() => 0.0f, 
        x => animParams.cubeRenderer.material.SetColor("_EmissionColor", Color.Lerp(currentScannerColor, animParams.color.invalid, (float)Math.Sin(x))), 
        Math.PI, 
        animParams.time.wrong)
        .SetEase(Ease.Linear)
        .SetId(animParams.cubeAnimator);

        DOTween.To(() => 0.0f,
        x => animParams.cubeAnimator.speed = 1.0f + (float)Math.Sin(x) * 3.0f, 
        Math.PI, 
        animParams.time.wrong)
        .SetEase(Ease.Linear)
        .SetId(animParams.cubeAnimator);

        // DOTween.To(() => 0.0f, 
        // x => anim.screenRenderer.material.SetColor("_EmissionColor", Color.Lerp(defaultCubeColor, anim.color.invalid, (float)Math.Sin(x))), 
        // Math.PI, 
        // anim.time.wrong)
        // .SetEase(Ease.Linear)
        // .SetId(anim.cubeAnimator);
        
        // Won't be changed in update before tweens above end
        animParams.screenRenderer.material.SetColor("_EmissionColor", animParams.color.invalid);
        animParams.screenRenderer.material.SetTexture("_EmissionMap", animParams.textures.invalid);
    }

    void animOnScan()
    {
        if (DOTween.IsTweening(animParams.cubeAnimator))
            return;

        // DOTween.To(() => 0.0f, 
        // x => animParams.cubeRenderer.material.SetColor("_EmissionColor", Color.Lerp(currentScannerColor, animParams.color.scanning, (float)(Math.Sin(x) * Math.Min(x * 3.0f, 1.0f)))), 
        // Math.PI, 
        // animParams.time.scanning)
        // .SetEase(Ease.Linear)
        // .SetId(animParams.cubeAnimator);

        // DOTween.To(() => 0.0f,
        // x => animParams.cubeAnimator.speed = 1.0f + (float)Math.Sin(x) * 6.0f, 
        // Math.PI, 
        // animParams.time.scanning)
        // .SetEase(Ease.Linear)
        // .SetId(animParams.cubeAnimator);

         animParams.cubeRenderer.material.SetColor("_EmissionColor", animParams.color.scanning);

        DOTween.To(() => 1.0f,
        x => animParams.cubeAnimator.speed = Mathf.Lerp(animParams.minCubeSpeed, animParams.maxCubeSpeed, x), 
        0.0f, 
        animParams.time.scanning)
        .SetEase(Ease.Linear)
        .SetId(animParams.cubeAnimator);

        
        // DOTween.To(() => 0.0f, 
        // x => animParams.screenRenderer.material.SetColor("_EmissionColor", Color.Lerp(currentScannerColor, animParams.color.scanning, (float)(Math.Sin(x) * Math.Min(x * 3.0f, 1.0f)))), 
        // Math.PI, 
        // animParams.time.scanning)
        // .SetEase(Ease.Linear)
        // .SetId(animParams.cubeAnimator);
        
        animParams.screenRenderer.material.SetColor("_EmissionColor", animParams.color.scanning);

        int frames = animParams.textures.scanning.Length;
        DOTween.To(() => 0.0f, 
        x => animParams.screenRenderer.material.SetTexture("_EmissionMap", animParams.textures.scanning[(int)x % frames]),
        animParams.time.scanning * 11.99f,
        animParams.time.scanning)
        .SetEase(Ease.Linear)
        .SetId(animParams.cubeAnimator);
    }
    
    IEnumerator ScanCooldown(float time)
    {
        canScan = false;
        isScanning = false;
        scanningProgress = 0.0f;
        yield return new WaitForSeconds(time);
        canScan = true;
    }
}
