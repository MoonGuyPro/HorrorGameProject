using UnityEngine;

namespace AudioScripts.AmbientSounds
{
    public class AmbientSpookySounds : AbstractRandomCall
    {
        [SerializeField] private AudioClip[] clips;
        private AudioSource audioSource;
        protected override void Start()
        {
            base.Start();
            audioSource = GetComponent<AudioSource>();
        }

        protected override void OnInterval()
        {
            print("Playing soundsss");
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.Play();
        }
    }
}