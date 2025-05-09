using FMODUnity;
using UnityEngine;

public class TurnHead : MonoBehaviour
{
    public Transform target;
    [SerializeField]
    private int speed = 5;

    [SerializeField] 
    private EventReference sound;
    FMOD.Studio.EventInstance soundEvent;
    bool isPlaying = false;

    private void Awake()
    {
        soundEvent = RuntimeManager.CreateInstance(sound);
        soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && target != null)
        {
            if (!isPlaying)
            {
                soundEvent.start();
                isPlaying = true;
            }
            Vector3 relativePos = target.position - transform.position;
            if (relativePos != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                transform.rotation = rotation;
                Quaternion current = transform.rotation;

                transform.rotation = Quaternion.Slerp(current, rotation, 
                    Time.deltaTime * speed);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlaying)
        {
            soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isPlaying = false;
        }
    }
}
