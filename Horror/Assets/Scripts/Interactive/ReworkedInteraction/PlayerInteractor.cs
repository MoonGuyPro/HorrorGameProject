using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    public Transform PlayerCamera;
    [SerializeField]
    private float raycastDistance = 4f;
    
    [Header("Tip text")]
    public GameObject tipLabel;
    
    // Krystian - added inventory support
    [SerializeField] Inventory inv;
    [SerializeField] TextMeshProUGUI inventoryTextMesh;

    private TextMeshProUGUI textMesh;
    
    private bool alreadyLooking;
    private bool isInteracting;
    private InputAction interactAction;

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

    void OnEnable ()
    {
		InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>("NyctoInputActions");
		InputActionMap inputActionMap = inputActionAsset.FindActionMap("Player");
        interactAction = inputActionMap.FindAction("Interact");
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
                    if (alreadyLooking == false && interaction.CanInteract)
                    {
                        textMesh.text = interaction.GetTip();
                        alreadyLooking = true;
                    }
                    
                    if (interactAction.WasPressedThisFrame()) // Who tf at Unity called it like this?!
                    {
                        if(interaction.Interact(inv))
                        {
                            updateInventoryText();
                            alreadyLooking = false;
                            textMesh.text = "";
                        }
                        else
                        {
                            textMesh.text = interaction.GetAltTip();
                        }
                    }
                }
            }

            if (hit.transform.CompareTag("Pickable"))
            {
                Pickable pickable = hit.transform.GetComponentInParent<Pickable>();

                if (pickable is not null)
                {
                    if (alreadyLooking == false)
                    {
                        textMesh.text = pickable.Data.TipText;
                        alreadyLooking = true;
                    }
                    
                    if (interactAction.WasPressedThisFrame())
                    {
                        inv.addItem(pickable.Data);
                        pickable.PickUp();
                        updateInventoryText();
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

    void updateInventoryText()
    {
        if (!inv)
            return;
        
        string invContent = inv.printInGameNames();
        if (invContent == "Empty")
        {
            inventoryTextMesh.text = "";
            return;
        }
        inventoryTextMesh.text = "Inventory:\n" + invContent;
    }
}

