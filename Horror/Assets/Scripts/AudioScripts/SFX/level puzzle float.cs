using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelpuzzlefloat : MonoBehaviour
{
    private Animator a;
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (a is not null)
        {
            
        }
    }
}
