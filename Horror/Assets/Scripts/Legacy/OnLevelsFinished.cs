using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class does something when you trigger a finish of a sub-level
public class OnLevelsFinished : MonoBehaviour
{
    private int initialValue;
    void Start()
    {
        initialValue = StaticGlobalVariables.GetGatesOpened();
    }

    // TODO: change how this checks for value. use: https://answers.unity.com/questions/1206632/trigger-event-on-variable-change.html
    void Update()
    {
        if (StaticGlobalVariables.GetGatesOpened() != initialValue)
        {
            GetComponent<Animator>().SetBool("used", true);
        }
    }
}
