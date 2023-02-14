using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FPSController : PortalTraveller {

    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float smoothMoveTime = 0.1f;
    public float jumpForce = 8;
    public float gravity = 18;

    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Vector2 pitchMinMax = new Vector2 (-90, 90); // Change to limit camera angles (currently no limit)
    public float rotationSmoothTime = 0.1f;

    CharacterController controller;
    Camera cam;
    public float yaw;
    public float pitch;
    float smoothYaw;
    float smoothPitch;

    float yawSmoothV;
    float pitchSmoothV;
    float verticalVelocity;
    Vector3 velocity;
    Vector3 smoothV;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    bool jumping;
    float lastGroundedTime;
    bool disabled;

    // These events were created for audio triggering purposes.
    // Using this is much simpler then (as before) checking all parameters in Update()
    [SerializeField] 
    private UnityEvent OnWalking;
    // this event will trigger on stopping or jumping. Just to stop the footsteps playback.
    // possible later split into two events stop and jump.
    [SerializeField]
    private UnityEvent OnStopWalking;

    [HideInInspector] public bool bStuck;

    void Start () {
        cam = Camera.main;
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        controller = GetComponent<CharacterController> ();

        yaw = transform.eulerAngles.y;
        pitch = cam.transform.localEulerAngles.x;
        smoothYaw = yaw;
        smoothPitch = pitch;
    }

    void Update () {
        if (Input.GetKeyDown (KeyCode.P)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Break ();
        }
        if (Input.GetKeyDown (KeyCode.O)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            disabled = !disabled;
        }

        if (disabled) {
            return;
        }

        Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
        
        Vector3 inputDir = new Vector3 (input.x, 0, input.y).normalized;
        Vector3 worldInputDir = transform.TransformDirection (inputDir);

        float currentSpeed = (Input.GetKey (KeyCode.LeftShift)) ? runSpeed : walkSpeed;
        Vector3 targetVelocity = worldInputDir * currentSpeed;
        velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);
        if (velocity.magnitude > 1f && jumping == false && !bStuck) 
        {
            OnWalking.Invoke();
        } else if (velocity.magnitude < 1f || jumping || bStuck)
        {
            OnStopWalking.Invoke();
        }

        verticalVelocity -= gravity * Time.deltaTime;
        
        velocity = new Vector3 (velocity.x, verticalVelocity, velocity.z);

        // Stop player if he's stuck
        if (!bStuck)
        {
            var flags = controller.Move(velocity * Time.deltaTime);
            if (flags == CollisionFlags.Below)
            {
                jumping = false;
                lastGroundedTime = Time.time;
                verticalVelocity = 0;
            }
        }

        if (Input.GetKeyDown (KeyCode.Space)) {
            float timeSinceLastTouchedGround = Time.time - lastGroundedTime;
            if (controller.isGrounded || (!jumping && timeSinceLastTouchedGround < 0.15f)) {
                jumping = true;
                verticalVelocity = jumpForce;
            }
        }

        float mX = Input.GetAxisRaw ("Mouse X");
        float mY = Input.GetAxisRaw ("Mouse Y");

        // Verrrrrry gross hack to stop camera swinging down at start
        float mMag = Mathf.Sqrt (mX * mX + mY * mY);
        if (mMag > 5) {
            mX = 0;
            mY = 0;
        }

        yaw += mX * mouseSensitivity;
        pitch -= mY * mouseSensitivity;
        pitch = Mathf.Clamp (pitch, pitchMinMax.x, pitchMinMax.y);
        smoothPitch = Mathf.SmoothDampAngle (smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
        smoothYaw = Mathf.SmoothDampAngle (smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);

        transform.eulerAngles = Vector3.up * yaw;
        cam.transform.localEulerAngles = Vector3.right * pitch;

    }

    public override void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        transform.position = pos;
        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle (smoothYaw, eulerRot.y);
        yaw += delta;
        smoothYaw += delta;
        transform.eulerAngles = Vector3.up * smoothYaw;
        velocity = toPortal.TransformVector (fromPortal.InverseTransformVector (velocity));
        Physics.SyncTransforms ();
    }

}