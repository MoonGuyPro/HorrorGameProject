using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public Transform PlayerCamera;
    [Header("Max distance of interaction")]
    public float MaxDistance = 5;
    [Header("Tip text")]
    public GameObject tipLabel;

    public Inventory inv;
    
    private TextMeshProUGUI textMesh;
    private Interactive interactive;
    private Pickable pickable;

    private bool alreadyLooking;

    // If tipLabel is null show warning
    void Start()
    {
        if(tipLabel == null)
        {
            Debug.LogWarning("PlayerInteractive.cs: tipText is null - cannot show interaction tips!");
        }
        else
        {
            textMesh = tipLabel.GetComponent<TextMeshProUGUI>();
        }

        alreadyLooking = false;
    }

    void Update()
    {
        // Check if player looks at interactive or pickable object
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, MaxDistance))
        {
            if (hit.transform.tag == "Interactive")
            {
                // Show tip on screen
                interactive = hit.transform.GetComponentInParent<Interactive>();

                // disabled objects dont trigger anything, such as used key holes that are already used.
                if (interactive.isActive)
                {
                    if (!alreadyLooking)
                    {
                        setTipText(interactive.tip);
                    }

                    toggleTipText(true);

                    // On interact key
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        // Call interaction
                        interactive = hit.transform.GetComponentInParent<Interactive>();
                        if (!interactive.Interact())
                        {
                            setTipText(interactive.altTip);
                        }
                    }
                    alreadyLooking = true;
                } 
            }
            if (hit.transform.tag == "Pickable")
            {
                pickable = hit.transform.GetComponentInParent<Pickable>();
                setTipText(pickable.tip);
                toggleTipText(true);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    // Call interaction
                    pickable = hit.transform.GetComponentInParent<Pickable>();
                    inv.addItem(pickable);
                    pickable.interact();
                }
                alreadyLooking = true;
            }
        }
        else
        {
            toggleTipText(false);
            alreadyLooking = false;
        }
        
    }

    // Just in case check if tipLabel is null
    void setTipText(string tip)
    {
        if(tipLabel != null)
            textMesh.text = tip + " [F]";
    }

    void toggleTipText(bool enabled)
    {
        if(tipLabel != null)
            tipLabel.SetActive(enabled);
    }
}
