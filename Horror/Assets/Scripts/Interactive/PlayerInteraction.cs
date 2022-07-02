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
    
    private TextMeshProUGUI textMesh;
    private Interactive interactive;

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
    }

    void Update()
    {
        // Check if player looks at interactive object
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, MaxDistance))
        {
            if (hit.transform.tag == "Interactive")
            {
                // Show tip on screen
                interactive = hit.transform.GetComponentInParent<Interactive>();
                setTipText(interactive.tip);
                toggleTipText(true);

                // On interact key
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // Call interaction
                    interactive = hit.transform.GetComponentInParent<Interactive>();
                    interactive.interact();
                }
            }
        }
        else
        {
            toggleTipText(false);
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
