using UnityEngine;

public class Key : Pickable
{
    private void Start()
    {
        objTransform = transform;
        audioSource = GetComponent<AudioSource>();

        // if no sounds were provided we load some default sounds
        
        
        if (pickUpSounds.Length == 0)
        {
            pickUpSounds = new AudioClip[3];
            
            pickUpSounds[0] = Resources.Load<AudioClip>("variousFX/Items/PickUp1");
            pickUpSounds[1] = Resources.Load<AudioClip>("variousFX/Items/PickUp2");
            pickUpSounds[2] = Resources.Load<AudioClip>("variousFX/Items/PickUp3");
        }
    }

    public override void interact()
    {
        objTransform.position -= new Vector3(0,10,0);
        int soundNum = Random.Range(0, pickUpSounds.Length);
        print(soundNum.ToString());
        audioSource.clip = pickUpSounds[soundNum];
        audioSource.Play();
    }

    public void PlaceKey(KeyHole obj)
    {
        objTransform.position = obj.transform.position;
    }
}
