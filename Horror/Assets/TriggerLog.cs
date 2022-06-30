using UnityEngine;

public class TriggerLog : InputLogic
{
    private AudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    protected override void behavior()
    {
        m_AudioSource.Play();
    }
}
