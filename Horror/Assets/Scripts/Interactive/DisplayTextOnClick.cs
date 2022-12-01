using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayTextOnClick : Interactive
{
    public string textValue;
    public GameObject textElement;
    public int time;

    void Start()
    {
        textElement.SetActive(false);
    }

    public override bool Interact(Inventory inv)
    {
        if (textElement != null)
        {
            textElement.GetComponent<TextMeshProUGUI>().text = textValue;
            textElement.SetActive(true);
            StartCoroutine("Wait");
        }
        return true;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(time);
        textElement.SetActive(false);
    }

}
