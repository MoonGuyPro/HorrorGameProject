using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrregularBlinking : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetFloat("Offset", Random.Range(0.0f, 1.0f));
    }
}
