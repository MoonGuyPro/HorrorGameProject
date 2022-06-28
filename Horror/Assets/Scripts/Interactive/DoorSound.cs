using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSound : MonoBehaviour
{
    private AudioSource[] s;
    private int clip;
    private SlideDoor sd;
    private bool isChanged;

    // Start is called before the first frame update
    void Start()
    {
        s = GetComponents<AudioSource>();
        clip = 0;
        sd = GetComponent<SlideDoor>();
        isChanged = sd.animator.GetBool("active");
    }

    // Update is called once per frame
    void Update()
    {
        if (sd.animator.GetBool("active") != isChanged)
        {
            s[clip].Play();
            clip ^= 1;
            isChanged = sd.animator.GetBool("active");
        }
    }
}
