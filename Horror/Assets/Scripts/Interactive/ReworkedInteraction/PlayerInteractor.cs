using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public Transform PlayerCamera;
    [SerializeField]
    private float raycastDistance = 4f;
    
    [Header("Tip text")]
    public GameObject tipLabel;
    
    private TextMeshProUGUI textMesh;
    
    private bool alreadyLooking;

    void Start()
    {
        if(tipLabel == null) // If tipLabel is null show warning
        {
            Debug.LogWarning("PlayerInteractor.cs: tipText is null - cannot show interaction tips!");
        }
        else
        {
            textMesh = tipLabel.GetComponent<TextMeshProUGUI>();
        }
        
        alreadyLooking = false;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, raycastDistance))
        {
            if (hit.transform.CompareTag("Interactive"))
            {
                InteractionInput interaction = hit.transform.GetComponentInParent<InteractionInput>();

                if (interaction is not null)
                {
                    if (alreadyLooking == false)
                    {
                        textMesh.text = interaction.GetTip();
                        alreadyLooking = true;
                    }
                    
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        interaction.Interact();
                    }
                }
            }
            
        }
        else
        {
            alreadyLooking = false;
            textMesh.text = "";
        }
    }
}

