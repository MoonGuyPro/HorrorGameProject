using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayTextTrigerTouch : MonoBehaviour
{
    public string textValue;
    public GameObject textElement;
    void Start()
    {
        textElement.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (textElement != null)
        {
            textElement.GetComponent<TextMeshProUGUI>().text = textValue;
            textElement.SetActive(true);
        }
    }

    private void OnMouseUp()
    {
        textElement.SetActive(false);
    }
}
