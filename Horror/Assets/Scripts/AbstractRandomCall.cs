using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRandomCall : MonoBehaviour
{   
    public float minInterval;
    public float maxInterval;

    private float targetTime;

    void Start() {
        targetTime = Random.Range(minInterval, maxInterval);    
    }

    void Update()
    {
        targetTime -= Time.deltaTime;
        if(targetTime <= 0.0f) {
            targetTime = Random.Range(minInterval, maxInterval);
            onInterval();
        }
    }

    protected abstract void onInterval();
}
