using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Pickable
{
    private void Start()
    {
        objTransform = obj.transform;
    }

    public override void interact()
    {
        objTransform.position -= new Vector3(0,10,0);
    }
}
