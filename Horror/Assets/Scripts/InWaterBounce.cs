using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWaterBounce : MonoBehaviour
{
    [SerializeField] private float strength = 0.5f;
    [SerializeField] private float speed = 0.5f;
    private float time = 0f;
    private float originalY;

    private void Start()
    {
        originalY = transform.position.y;
    }

    void Update()
    {
        time += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, originalY + Mathf.Sin(time * speed) * strength, transform.position.z);
    }
}
