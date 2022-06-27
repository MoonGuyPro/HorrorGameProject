using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InputLogic
{
    protected override void behavior()
    {
        print("Lever: " + active);
    }
}
