using UnityEngine;

public class TriggerLog : Interactive
{
    private AudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public override bool Interact()
    {
        m_AudioSource.Play();
        return true;
    }
}
