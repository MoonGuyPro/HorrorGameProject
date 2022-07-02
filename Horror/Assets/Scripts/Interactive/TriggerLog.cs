using UnityEngine;

public class TriggerLog : MonoBehaviour, Interactive
{
    private AudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void interact()
    {
        m_AudioSource.Play();
    }
}
