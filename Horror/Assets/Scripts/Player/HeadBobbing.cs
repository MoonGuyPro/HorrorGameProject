using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] bool useHeadBobbing = true;
    [Tooltip("How fast the head bobs when walking in Hz")]
    [SerializeField] float bobbingWalkSpeed = 2f;
    [Tooltip("How fast the head bobs when walking in Hz")]
    [SerializeField] float bobbingRunSpeed = 3f;
    [SerializeField] float bobbingAmount = 0.2f;
    float defaultY = 0.0f;
    float timer = 0.0f;

    bool isWalking = true;
    
    // Start is called before the first frame update
    void Start()
    {
        defaultY = cameraTransform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (useHeadBobbing)
        {
            HandleHeadbob();
        }
    }

    void HandleHeadbob()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            if (isWalking)
            {
                timer += Time.deltaTime * bobbingWalkSpeed * 2f * 3.1415f; 
            }
            else
            {
                timer += Time.deltaTime * bobbingRunSpeed * 2f * 3.1415f; 
            }
            
            if (timer > Mathf.PI * 2)
            {
                timer -= Mathf.PI * 2;
            }
        }

        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange *= totalAxes;
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, defaultY + translateChange, cameraTransform.localPosition.z);
        }
        else
        {
            //cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, defaultY, cameraTransform.localPosition.z);
            
            // lerp y position back to default
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, new Vector3(cameraTransform.localPosition.x, defaultY, cameraTransform.localPosition.z), Time.deltaTime * 2f);
        }
    }

    public void setRunning()
    {
        isWalking = false;
    }
    
    public void setWalking()
    {
        isWalking = true;
    }
}
