using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FPSController : PortalTraveller
{

    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float smoothMoveTime = 0.1f;
    //public float jumpForce = 8;
    public float gravity = 20;
    //public float maxYawJumping = 60;

    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Vector2 pitchMinMax = new Vector2(-90, 90); // Change to limit camera angles (currently no limit)
    public float rotationSmoothTime = 0.1f;

    CharacterController controller;
    Camera cam;
    public float yaw;
    public float pitch;
    public float staringYaw;
    float smoothYaw;
    float smoothPitch;


    float currentSpeed;
    float yawSmoothV;
    float pitchSmoothV;
    float verticalVelocity;
    float sensitivity = 0.8f; //This sensitivity is retreived from PlayerPrefs (Settings)
    Vector3 velocity;
    Vector3 lastTargetVelocity;
    Vector3 smoothV;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    Action readLookAndMove;
    InputAction sprintAction;
    Vector2 lookInput;
    Vector2 moveInput;

    private bool invertX = false;
    private bool invertY = false;
    bool isJumping;
    float lastGroundedTime;
    bool disabled;

    // These events were created for audio triggering purposes.
    // Using this is much simpler then (as before) checking all parameters in Update()
    [SerializeField]
    private UnityEvent OnStartWalking;

    [SerializeField]
    private UnityEvent OnRunning;
    [SerializeField]
    private UnityEvent OnWalking;
    // this event will trigger on stopping or jumping. Just to stop the footsteps playback.
    // possible later split into two events stop and jump.
    [SerializeField]
    private UnityEvent OnStopWalking;

    [HideInInspector] public bool bStuck = false;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        LockCursor(lockCursor);

        controller = GetComponent<CharacterController>();

        yaw = transform.eulerAngles.y;
        if (cam)
        {
            pitch = cam.transform.localEulerAngles.x;
        }
        smoothYaw = yaw;
        smoothPitch = pitch;

        currentSpeed = walkSpeed;
    }

    void OnEnable()
    {
        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
        InputActionMap inputActionMap = inputActionAsset.FindActionMap("Player");
        readLookAndMove = () =>
        {
            moveInput = inputActionMap["Move"].ReadValue<Vector2>();
            lookInput = inputActionMap["Look"].ReadValue<Vector2>();
        };
        inputActionMap.FindAction("LockCursor").performed += LockCursor;
        sprintAction = inputActionMap.FindAction("Sprint");
        //inputActionMap.FindAction("Jump").performed += Jump;
    }

    void OnDisable()
    {
        InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
        InputActionMap inputActionMap = inputActionAsset.FindActionMap("Player");
        readLookAndMove = null;
        inputActionMap.FindAction("LockCursor").performed -= LockCursor;
        //inputActionMap.FindAction("Jump").performed -= Jump;
    }

    void Update()
    {
        if (disabled)
            return;

        UpdatePrefs(); //todo: move it somewhere else to not call it every frame
        readLookAndMove.Invoke();

        if (!isJumping) // Allow horizontal movement during walking
        {
            currentSpeed = sprintAction.IsPressed() ? runSpeed : walkSpeed;
        }

        Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        Vector3 worldInputDir = transform.TransformDirection(inputDir);

        Vector3 targetVelocity = worldInputDir * currentSpeed;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothV, smoothMoveTime);
        
        if (controller.velocity.sqrMagnitude > 1f && !isJumping && !bStuck)
        {
            if (currentSpeed > runSpeed - 0.1)
            {
                OnRunning.Invoke();
            }
            else
            {
                OnWalking.Invoke();
            }

            OnStartWalking.Invoke();
        }
        else if (controller.velocity.sqrMagnitude < 1f || isJumping || bStuck)
        {
            OnStopWalking.Invoke();
        }
        verticalVelocity -= gravity * Time.deltaTime;
        velocity = new Vector3(velocity.x, verticalVelocity, velocity.z);

        if (!bStuck)
        {
            Vector3 moveValue = velocity * Time.deltaTime;
            CollisionFlags flags = controller.Move(moveValue);
            if (moveValue.sqrMagnitude > 0.1f) // possible that very slight slopes may cause constant footsteps? This should prevent them
            {
                if ((flags & CollisionFlags.Below) != 0)
                {
                    isJumping = false;
                    lastGroundedTime = Time.time;
                    verticalVelocity = 0;
                }
            }
        }

        float mX = lookInput.x;
        float mY = lookInput.y;

        // Verrrrrry gross hack to stop camera swinging down at start
        // Kris here - mMag can reach very high values at low framerate.
        /*float mMag = Mathf.Sqrt (mX * mX + mY * mY);
        if (mMag > 5) {
            mX = 0;
            mY = 0;
        } */
        if (PauseMenu.IsPaused) return;

        float rotSensitivity = mouseSensitivity * sensitivity;

        yaw += invertX ? -mX * rotSensitivity : mX * rotSensitivity;
        pitch -= invertY ? -mY * rotSensitivity : mY * rotSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
        smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);

        transform.eulerAngles = Vector3.up * yaw;
        cam.transform.localEulerAngles = Vector3.right * pitch;
    }

    public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle(smoothYaw, eulerRot.y);
        yaw += delta;
        smoothYaw += delta;
        transform.eulerAngles = Vector3.up * smoothYaw;
        velocity = toPortal.TransformVector(fromPortal.InverseTransformVector(velocity));
        Physics.SyncTransforms();
    }

    void LockCursor(bool value)
    {
        lockCursor = value;
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
        // Debug.Break(); // Was it important? -KB
    }

    void LockCursor(InputAction.CallbackContext context)
    {
        lockCursor = !lockCursor;
        LockCursor(!lockCursor);
    }

    /*void Jump (InputAction.CallbackContext context)
    {
        float timeSinceLastTouchedGround = Time.time - lastGroundedTime;
        if (controller.isGrounded || (!isJumping && timeSinceLastTouchedGround < 0.15f))
        {
            staringYaw = yaw;
            isJumping = true;
            verticalVelocity = jumpForce;
        }
    }*/

    private void UpdatePrefs()
    {
        sensitivity = PlayerPrefs.GetFloat("sensitivity", 0.8f);
        invertX = PlayerPrefs.GetInt("invertXAxis", 0) == 1;
        invertY = PlayerPrefs.GetInt("invertYAxis", 0) == 1;
    }
}