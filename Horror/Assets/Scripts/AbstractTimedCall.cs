using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Base class for timed function calls.
 * Inherit this class and change the onInterval funcion to call it with specified intervals.
 * If needed the Start() method is also made virtual so it can be overriden, BUT base.Start() MUST BE CALLED!
 * (see AudioScripts/AmbientSounds/AmbientSpookySounds.cs for proper simple use of this class)
 */
public abstract class AbstractTimedCall : MonoBehaviour
{   
    public float minInterval;
    public float maxInterval;
    /** Fixed interval means all action will take place with on a fixed interval, using minInterval value.
     * Otherwise calls will happen at random in (minInterval, maxInterval) seconds.
     */
    public bool fixedInterval;

    /** Should the call be made a fixed amount of times?
     */
    public bool callNTimes;
    /** How many times should the call be made? (if callNTimes is checked)
     */
    public int nTimes;

    private float targetTime;
    private bool firstCall;

    // using Coroutines for timed events
    IEnumerator randomTimedCall()
    {
        if (callNTimes)
        {
            for (int i = 0; i < nTimes + 1; i++)
            {
                prepareTimer(firstCall);
                if (firstCall)
                {
                    firstCall = false;
                    yield return new WaitForSeconds(targetTime);
                } 
                OnInterval();
                yield return new WaitForSeconds(targetTime);
            }
        }
        else
        {
            // runs indefinitely
            while (true)
            {
                prepareTimer(firstCall);

                // we're skipping first call because the inherited class' Start() method has not been called yet.
                if (firstCall)
                {
                    firstCall = false;
                    yield return new WaitForSeconds(targetTime);
                }
                else
                {
                    OnInterval();
                    yield return new WaitForSeconds(targetTime);
                }
                
            }
        }
        
    }
    
    protected virtual void Start()
    {
        firstCall = true;
        StartCoroutine(randomTimedCall());
    }

    protected abstract void OnInterval();

    // prepares the timer for next interval
    private void prepareTimer(bool firstCall)
    {
        if (fixedInterval)
        {
            // fixed amount of time between all calls
            targetTime = minInterval;
        }
        else
        {
            if (firstCall)
            {
                targetTime = Random.Range(0.1f, maxInterval);
            }
            else
            {
                targetTime = Random.Range(minInterval, maxInterval);
            }
        }
    }
}
