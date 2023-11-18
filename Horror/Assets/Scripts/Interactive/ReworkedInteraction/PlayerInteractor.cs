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
    
    // Krystian - added inventory support
    [SerializeField] NewInventory inv;
    [SerializeField] TextMeshProUGUI inventoryTextMesh;

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
                        if(interaction.Interact(inv))
                        {
                            updateInventoryText();
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
                    
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        inv.addItem(pickable.Data);
                        pickable.PickUp();
                        updateInventoryText();
                    }
                }
            }

            // if (hit.transform.CompareTag("Pickable"))
            // {
            //     Pickable pickable = hit.transform.GetComponentInParent<Pickable>();

            //     if (pickable is not null)
            //     {
            //         if (alreadyLooking == false)
            //         {
            //             textMesh.text = pickable.Data.TipText;
            //             alreadyLooking = true;
            //         }
                    
            //         if (Input.GetKeyDown(KeyCode.F))
            //         {
            //             inv.addItem(pickable.Data);
            //             updateInventoryText();
            //         }
            //     }

            //     setTipText(pickable.tip);
            //     toggleTipText(true);
    
            //     if (Input.GetButtonDown("Interact"))
            //     {
            //         // Call interaction
            //         pickable = hit.transform.GetComponentInParent<Pickable>(); //isn't it redundant?
            //         if(hit.collider.name == "Supercube")
            //             uIPortal.getCube();
            //         else
            //             inv.addItem(pickable);
            //         updateInventoryText();
            //         pickable.interact();
            //     }
            //     alreadyLooking = true;
            // }
        }
        else
        {
            alreadyLooking = false;
            textMesh.text = "";
        }
    }

    void updateInventoryText()
    {
        string invContent = inv.printInGameNames();
        if (invContent == "Empty")
        {
            inventoryTextMesh.text = "";
            return;
        }
        inventoryTextMesh.text = "Inventory:\n" + invContent;
    }
}

