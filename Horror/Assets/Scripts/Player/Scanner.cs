using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using FMOD.Studio;
using UnityEngine.InputSystem;
using FMODUnity;
using UnityEngine.UI;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class Scanner : MonoBehaviour
{
    [SerializeField] bool playerHasScanner;

    [Serializable]
    class InputParams
    {
        public float scanTime = 2.0f, maxDistance = 5.0f, radius = 0.45f;
    }

    [Serializable]
    class UIParams
    {
        public TextMeshProUGUI subtitles, popupName, popupDescription;
        public Image popupPanel;
        public Image audioIcon;
        public LineRenderer lineRenderer;
        public Transform lineStart;
        public DisplaySubtitles displaySubtitles;
        public float displayingTime = 4.0f;
    }

    [Serializable]
    class AnimParams
    {
        public Animator scannerAnimator, cubeAnimator;
        public MeshRenderer cubeRenderer, screenRenderer;
        public float cubeAcceleration = 3.0f;
        public float minCubeSpeed = 1.0f, maxCubeSpeed = 20.0f;
        public AnimColors color;
        [Serializable]
        public class AnimColors
        {
            public Color normal = Color.cyan, invalid = Color.red, hover = Color.yellow, scanning = Color.green;
        }
        public AnimTimes time;
        [Serializable]
        public class AnimTimes
        {
            public float wrong = 0.3f, scanning = 2.0f, nameLetterScale = 1.0f, descriptionLetterScale = 0.25f;
        }
        public AnimTextures textures;
        [Serializable]
        public class AnimTextures
        {
            public Texture2D normal, invalid, hover;
            public Texture2D[] scanning;
        }

    }

    [Serializable]
    class SoundParams
    {
        public EventReference scanLetterSound, scannerDrawSound, scannerHideSound, noScanTargetSound, ScanningSound;
    }

    [SerializeField] Transform playerCamera;
    [SerializeField] InputParams inputParams;
    [SerializeField] UIParams uiParams;
    [SerializeField] AnimParams animParams;
    [SerializeField] SoundParams soundParams;
    [SerializeField] LayerMask raycastMask;
    private Color popupPanelColor;
    private bool isScannerEquipped = false;
    private bool canScan = true;
    private Transform lineEnd;
    public List<ScannableData> alreadyScanned;
    bool bDisplaying = false;
    Coroutine displayCoroutine;
    Tween displayTween;
    Scannable scannable;
    Vector3 scannableHitPos;
    Color scannabledDefaultEmissive;
    Color currentScannerColor; // Normal or hover
    Texture2D currentScreenTexture;
    Coroutine scanCooldownCoroutine;
    bool isScanning = false;
    float scanningProgress = 0.0f;

    private EventInstance scanningInstance; // this is just so i can stop the scanning sound when player stops scanning
    private EventInstance talkInstance;
    void Start()
    {
        popupPanelColor = uiParams.popupPanel.color;

        uiParams.subtitles.color = Color.clear;
        uiParams.popupName.color = Color.clear;
        uiParams.popupPanel.color = Color.clear;
        uiParams.audioIcon.color = Color.clear;
        uiParams.popupDescription.color = Color.clear;
        uiParams.lineRenderer.material.color = Color.clear;

        uiParams.lineRenderer.SetPosition(0, uiParams.lineStart.position);
        uiParams.lineRenderer.enabled = false;

        currentScannerColor = animParams.color.normal;

        scanningInstance = RuntimeManager.CreateInstance(soundParams.ScanningSound);

        if (alreadyScanned == null)
        {
            alreadyScanned = new List<ScannableData>();
        }

        if (playerHasScanner)
        {
            isScannerEquipped = true;
            animParams.scannerAnimator.SetBool("draw", isScannerEquipped);
            StartCoroutine(ScannerSoundWithDelay(isScannerEquipped));
        }
    }

    void OnEnable()
    {
        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
        InputActionMap inputActionMap = inputActionAsset.FindActionMap("Player");
        inputActionMap.FindAction("Scan").performed += OnScanPressed;
        inputActionMap.FindAction("Scan").canceled += OnScanReleased;
        inputActionMap.FindAction("Equip").performed += EquipScanner;
    }

    void OnDisable()
    {
        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
        InputActionMap inputActionMap = inputActionAsset.FindActionMap("Player");
        inputActionMap.FindAction("Scan").performed -= OnScanPressed;
        inputActionMap.FindAction("Scan").canceled -= OnScanReleased;
        inputActionMap.FindAction("Equip").performed -= EquipScanner;
    }

    // Call this when player finds scanner
    public void OnScannerPickedUp()
    {
        playerHasScanner = true;
        isScannerEquipped = true;
        animParams.scannerAnimator.SetBool("draw", isScannerEquipped);
        StartCoroutine(ScannerSoundWithDelay(isScannerEquipped));

        uiParams.displaySubtitles.Display("Press V/RMB to scan objects");
        // uiParams.subtitles.text = "Press V/RMB to scan objects";
        // DOTween.To(() => 0.0f, x => uiParams.subtitles.color = new Color(1.0f, 1.0f, 1.0f, x), 10.0f, 10.0f)
        //     .OnComplete(() => uiParams.subtitles.color = Color.clear);
    }

    void EquipScanner(InputAction.CallbackContext context)
    {
        if (!playerHasScanner)
            return;
        isScannerEquipped = !isScannerEquipped;
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
                RuntimeManager.PlayOneShot(soundParams.noScanTargetSound);
                scanCooldownCoroutine = StartCoroutine(ScanCooldown(animParams.time.wrong));
            }
        }
        else
        {
            isScanning = true;
            scanningInstance.start();
        }
    }

    void OnScanReleased(InputAction.CallbackContext context)
    {
        isScanning = false;
        scanningInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    void OnScanned()
    {
        if (!DOTween.IsTweening(animParams.cubeAnimator))
        {
            animOnScan();

            if (bDisplaying)
                StopCoroutine(displayCoroutine);

            if (scannable.IsAudio) {
                RuntimeManager.PlayOneShotAttached(scannable.talkEvent, gameObject);
                talkInstance = RuntimeManager.CreateInstance(scannable.talkEvent);
                EventDescription desc;
                int newLength;
                talkInstance.getDescription(out desc);
                desc.getLength(out newLength);
                talkInstance.start();
                uiParams.displayingTime = newLength/1000.0f;
                uiParams.audioIcon.color = Color.white;
            }

            displayCoroutine = StartCoroutine(
                DisplayPopupNoTweening(scannable.Data.DisplayName, scannable.Data.Description, uiParams.displayingTime, scannableHitPos));

            scannable.OnScanned?.Invoke();

            scanCooldownCoroutine = StartCoroutine(ScanCooldown(animParams.time.scanning));
        }
    }

    void Update()
    {
        if (!isScannerEquipped)
            return;

        if (Physics.SphereCast(playerCamera.position, inputParams.radius, playerCamera.forward, out RaycastHit hit, inputParams.maxDistance))
        {
            if (hit.transform.CompareTag("Scannable") || hit.transform.CompareTag("Interactive"))
            {
/*                if (scannable == null || scannable.transform != hit.transform)
                {
                    MeshRenderer mesh = hit.transform.GetComponentInParent<MeshRenderer>();
                    scannabledDefaultEmissive = mesh.material.GetColor("_EmissionColor");
                    //mesh.material.SetColor("_EmissionColor", animParams.color.hover);
                }*/

                scannable = hit.transform.GetComponentInParent<Scannable>();
                scannableHitPos = hit.point;
            }
        }
        else
        {
            if (scannable != null)
            {
                MeshRenderer mesh = scannable   .GetComponentInParent<MeshRenderer>();
                mesh.material.SetColor("_EmissionColor", scannabledDefaultEmissive);
            }

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

        if(!bDisplaying && scanningProgress >= 1.0f)
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

        // if (bDisplaying)
        // uiParams.lineRenderer.SetPosition(0, uiParams.lineStart.position);

        
 
    }

    IEnumerator DisplayPopupNoTweening(string name, string description, float fadeTime, Vector3 endPosition)
    {
        bDisplaying = true;
        //uiParams.lineRenderer.enabled = true;
        //uiParams.lineRenderer.SetPosition(0, uiParams.lineStart.position);
        //uiParams.lineRenderer.material.color = Color.clear;
        //uiParams.lineRenderer.SetPosition(1, endPosition);

        uiParams.popupName.text = "";
        uiParams.popupDescription.text = "";
        uiParams.popupName.color = Color.white;
        uiParams.popupDescription.color = Color.white;
        uiParams.popupPanel.color = popupPanelColor;

        var nameCount = name.Length;
        var descriptionCount = description.Length;

        var instance = RuntimeManager.CreateInstance(soundParams.scanLetterSound);
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
            uiParams.popupName.text += name[i];
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
            uiParams.popupDescription.text += description[i];
            instance.start();
            yield return new WaitForSeconds(speed);
        }

        yield return new WaitForSeconds(fadeTime);
        bDisplaying = false;
        if (!bDisplaying)
        {
            uiParams.popupName.color = Color.clear;
            uiParams.popupDescription.color = Color.clear;
            uiParams.popupPanel.color = Color.clear;
            uiParams.audioIcon.color = Color.clear;
            uiParams.lineRenderer.enabled = false;

        }
        // uiParams.popupName.color = Color.clear;
        // uiParams.popupDescription.color = Color.clear;
        // uiParams.popupPanel.color = Color.clear;
        // uiParams.lineRenderer.enabled = false;
    }

    IEnumerator LettersSounds(int lettersCount, float timeBetweenLetters)
    {
        for (int i = 0; i < lettersCount; i++)
        {
            RuntimeManager.PlayOneShot(soundParams.scanLetterSound);
            yield return new WaitForSeconds(timeBetweenLetters);
        }
    }

    IEnumerator DisplaySubtitles(string text, float displayTime, float fadeTime)
    {
        bDisplaying = true;
        uiParams.subtitles.text = text;
        uiParams.subtitles.color = Color.white;
        yield return new WaitForSeconds(displayTime);
        displayTween = uiParams.subtitles.DOColor(Color.clear, fadeTime);
        bDisplaying = false;
    }

    IEnumerator ScannerSoundWithDelay(bool equipped)
    {
        if (equipped)
        {
            yield return new WaitForSeconds(0.1f);
            RuntimeManager.PlayOneShot(soundParams.scannerDrawSound);
        }
        else
        {
            yield return new WaitForSeconds(0.35f);
            RuntimeManager.PlayOneShot(soundParams.scannerHideSound);
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
