using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform scannerSlotTransform;
    [SerializeField] bool useHeadBobbing = true;
    [Tooltip("How fast the head bobs when walking in Hz")]
    [SerializeField] float bobbingWalkSpeed = 2f;
    [Tooltip("How fast the head bobs when walking in Hz")]
    [SerializeField] float bobbingRunSpeed = 3f;
    [SerializeField] float bobbingAmount = 0.2f;
    [SerializeField] bool swayScanner = true;
    [SerializeField] float swaySpeed = 10.0f;
    [Tooltip("How fast scanner follows camera's rotation")]

    [SerializeField] float scannerBobbingMul = 0.5f;
    [Tooltip("How much headbobbing affects scanner")]
    float defaultY = 0.0f;
    float defaultScannerY = 0.0f;
    float timer = 0.0f;

    bool isWalking = true;
    
    // this is for other bobbing disabling events like jumping
    bool otherDisable = false;

    Vector3 lastCameraEulerAngles;
    Quaternion lastScannerRotation;

    Action readLook;
    Vector2 lookInput;
    
    // Start is called before the first frame update
    void Start()
    {
        defaultY = cameraTransform.localPosition.y;
        defaultScannerY = scannerSlotTransform.localPosition.y;
        lastCameraEulerAngles = cameraTransform.eulerAngles;
        lastScannerRotation = scannerSlotTransform.rotation;
    }

    void OnEnable () 
    {
		InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
		InputActionMap inputActionMap = inputActionAsset.FindActionMap("Player");
        readLook = () =>
        {
            lookInput = inputActionMap["Look"].ReadValue<Vector2>();
        };
    }
    
    void OnDisable ()
    {
		InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
		InputActionMap inputActionMap = inputActionAsset.FindActionMap("Player");
        readLook = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (useHeadBobbing)
        {
            HandleHeadbob();
        }
    }

    void FixedUpdate() 
    {
        if (swayScanner)
        {
            HandleScannerSway();
        }      
    }

    void HandleHeadbob()
    {
        float waveslice = 0.0f;
        float horizontal = lookInput.x;
        float vertical = lookInput.y;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = -Mathf.Pow(Mathf.Abs(Mathf.Sin(timer)), 1.5f);
            if (isWalking)
            {
                timer += Time.deltaTime * bobbingWalkSpeed * Mathf.PI; 
            }
            else
            {
                timer += Time.deltaTime * bobbingRunSpeed * Mathf.PI; 
            }
            
            if (timer > Mathf.PI * 2)
            {
                timer -= Mathf.PI * 2;
            }
        }

        if (waveslice != 0 && !otherDisable)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange *= totalAxes;
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, defaultY + translateChange, cameraTransform.localPosition.z);
            scannerSlotTransform.localPosition = new Vector3(scannerSlotTransform.localPosition.x, defaultScannerY + translateChange * scannerBobbingMul, scannerSlotTransform.localPosition.z);
        }
        else
        {
            // lerp y position back to default
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, new Vector3(cameraTransform.localPosition.x, defaultY, cameraTransform.localPosition.z), Time.deltaTime);
            scannerSlotTransform.localPosition = Vector3.Lerp(scannerSlotTransform.localPosition, new Vector3(scannerSlotTransform.localPosition.x, defaultScannerY, scannerSlotTransform.localPosition.z), Time.deltaTime);
        }
    }

    void HandleScannerSway()
    {
        scannerSlotTransform.rotation = Quaternion.Lerp(lastScannerRotation, cameraTransform.rotation, swaySpeed * 0.1f);
        lastScannerRotation = scannerSlotTransform.rotation;
    }

    public void SetRunning()
    {
        isWalking = false;
    }
    
    public void SetWalking()
    {
        isWalking = true;
    }

    public void SetStartWalking()
    {
        otherDisable = false;
    }

    public void SetDisable()
    {
        otherDisable = true;
    }
}
