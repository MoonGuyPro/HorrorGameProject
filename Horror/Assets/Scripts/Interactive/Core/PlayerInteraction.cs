using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public Transform PlayerCamera;
    [Header("SphereCast settings")]
    public float MaxDistance = 5;
    public float Radius = 0.45f;

    [Header("Tip text")]
    public GameObject tipLabel;

	[Header("Inventory text")]
    public GameObject invLabel;
    public Inventory inv;

    [Header("Supercube UI")]
    public UIPortal uIPortal;

    private TextMeshProUGUI textMesh;
    private TextMeshProUGUI inventoryTextMesh;
    private Interactive interactive;
    private Pickable pickable;

    private bool alreadyLooking;
    private const string interactionKey = "F"; //I will change this when we have the ability to edit keybinds.

    void Start()
    {
        if(tipLabel == null) // If tipLabel is null show warning
        {
            Debug.LogWarning("PlayerInteractive.cs: tipText is null - cannot show interaction tips!");
        }
        else
        {
            textMesh = tipLabel.GetComponent<TextMeshProUGUI>();
        }

        if (invLabel != null)
        {
            inventoryTextMesh = invLabel.GetComponent<TextMeshProUGUI>();
        }

        alreadyLooking = false;
    }

    void Update()
    {
        // Check if player looks at interactive or pickable object
        RaycastHit hit;
        if (Physics.SphereCast(PlayerCamera.transform.position, Radius, PlayerCamera.transform.forward, out hit, MaxDistance))
        {
            if (hit.transform.CompareTag("Interactive"))
            {
                // Show tip on screen
                interactive = hit.transform.GetComponentInParent<Interactive>();

                if (interactive is not null)
                {
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
                            interactive = hit.transform.GetComponentInParent<Interactive>(); //isn't it redundant?
                            if (!interactive.Interact(inv))
                            {
                                setTipText(interactive.altTip);
                            }
                            // we're calling inventory update here as well
                            // because we might've just used the key on something (e.g. KeyHole)
                            updateInventoryText();
                        }
                        alreadyLooking = true;
                    } 
                }
                else
                {
                    Debug.LogWarning("PlayerInteraction.cs: interactive is null");
                }
            }
            
            if (hit.transform.CompareTag("Pickable"))
            {
                pickable = hit.transform.GetComponentInParent<Pickable>();
                setTipText(pickable.tip);
                toggleTipText(true);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    // Call interaction
                    pickable = hit.transform.GetComponentInParent<Pickable>(); //isn't it redundant?
                    inv.addItem(pickable);
                    pickable.interact();
                    if(hit.collider.name == "Supercube")
                        uIPortal.getCube();
                    updateInventoryText();
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
        if (tipLabel)
        {
            textMesh.text = tip + " [" + interactionKey + "]";
        }
    }

    void toggleTipText(bool bEnabled)
    {
        if (tipLabel)
        {
            tipLabel.SetActive(bEnabled);
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
