using UnityEngine;

public class ClickLog : Interactive
{
    private AudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public override bool Interact(Inventory inv)
    {
        m_AudioSource.Play();
        return true;
    }
}
