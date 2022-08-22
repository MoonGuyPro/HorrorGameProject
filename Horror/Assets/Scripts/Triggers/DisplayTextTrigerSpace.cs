using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayTextTrigerSpace : MonoBehaviour
{

    public string textValue;
    public GameObject textElement;
    public int time;

    void Start()
    {
        textElement.SetActive(false);
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            textElement.GetComponent<TextMeshProUGUI>().text = textValue;
            textElement.SetActive(true);
            StartCoroutine("Wait");
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(time);
        textElement.SetActive(false);
        Destroy(gameObject);
    }

}
