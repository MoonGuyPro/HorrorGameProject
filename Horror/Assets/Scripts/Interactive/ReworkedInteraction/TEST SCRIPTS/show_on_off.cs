using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class show_on_off : MonoBehaviour
{
    public void On()
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
    
    public void Off()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
