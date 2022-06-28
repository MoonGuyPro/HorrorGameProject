using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leverSound : MonoBehaviour
{
    private AudioSource s;
    private int clip;
    private Lever sd;
    private int isChanged;

    // Start is called before the first frame update
    void Start()
    {
        s = GetComponent<AudioSource>();
        sd = GetComponent<Lever>();
        isChanged = sd.changed;
    }

    // Update is called once per frame
    void Update()
    {
        if (sd.changed != isChanged)
        {
            s.Play();
            isChanged = sd.changed;
        }
    }
}
