using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogTrigger : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public GameObject logObject;

    private AudioClip clip;
    private AudioSource source;

    private bool isPlaying;
    private bool startPlaying;

    // Start is called before the first frame update
    void Start()
    {
        source = logObject.GetComponent<AudioSource>();
        isPlaying = false;
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            startPlaying = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startPlaying)
        {
            if (!isPlaying)
            {
                source.Play();
                isPlaying = true;
            }
        }
    }
}
