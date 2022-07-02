using UnityEngine;

public class TriggerLog : Interactive
{
    private AudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public override void interact()
    {
        m_AudioSource.Play();
    }
}
