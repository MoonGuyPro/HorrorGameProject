using FMODUnity;
using UnityEngine;

public class KeyHole : InputLogic
{
    private int changed;
    public string keyName;
    public Transform keyModel;

    [SerializeField] private EventReference keyUseSound;

    private void Start()
    {
        isActive = true;
    }

    protected override void Behavior()
    {

    }

    public override bool Interact(Inventory inv)
    {
        Behavior(); // Call input behavior (implemented in extended class)

        if (inv.itemExists(keyName))
        {
            Toggle();

            keyModel.position += new Vector3(0, -10, 0);
            isActive = false;

            inv.removeItem(keyName);
            
            RuntimeManager.PlayOneShotAttached(keyUseSound, gameObject);
            
            return true;
        }
        return false;
    }
}
