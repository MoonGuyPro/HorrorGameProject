using UnityEngine;

public class KeyHole : InputLogic
{
    private int changed;
    public string keyName;
    public Transform keyModel;
    
    [SerializeField] protected AudioClip[] pickUpSounds;
    protected AudioSource audioSource;

    private void Start()
    {
        isActive = true;
        audioSource = GetComponent<AudioSource>();
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
            
            var soundNum = Random.Range(0, pickUpSounds.Length);
            audioSource.clip = pickUpSounds[soundNum];
            audioSource.Play();
            
            return true;
        }
        return false;
    }
}
