using FMODUnity;
using UnityEngine;

public class ShowOnItem : InputLogic
{
    private int changed;

    [Header("- ShowOnItem -")]
    public GameObject objectToShow;

    [SerializeField] private EventReference soundEvent;

    private void Start()
    {
        isActive = true;
    }

    protected override void Behavior()
    {
        objectToShow.SetActive(true);
    }

    // public override bool Interact(Inventory inv)
    // {
    //     Behavior(); // Call input behavior (implemented in extended class)

    //     if (inv.itemExists(keyName))
    //     {
    //         Toggle();

    //         keyModel.position += new Vector3(0, -10, 0);
    //         isActive = false;

    //         inv.removeItem(keyName);
            
    //         RuntimeManager.PlayOneShotAttached(keyUseSound, gameObject);
            
    //         return true;
    //     }
    //     return false;
    // }
}
