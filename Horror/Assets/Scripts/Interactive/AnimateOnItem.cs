using FMODUnity;
using UnityEngine;

public class AnimateOnItem : Interactive
{
    private int changed;
    
    [Header("Required item name")]
    public string itemName;
    
    [Header("Animator")]
    public Animator anim;
    public string boolName = "active";

    [SerializeField] private EventReference plantSound;
    public override bool Interact(Inventory inv)
    {
        if (inv.itemExists(itemName))
        {
            isActive = false;
            inv.removeItem(itemName);
            RuntimeManager.PlayOneShotAttached(plantSound, gameObject);
            anim.SetBool(boolName, true);
            
            return true;
        }
        return false;
    }
}