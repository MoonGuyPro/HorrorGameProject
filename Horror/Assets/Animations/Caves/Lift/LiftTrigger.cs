using UnityEngine;

public class LiftTrigger : MonoBehaviour
{
    private Transform _lift;

    private void Awake()
    {
        _lift = transform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent = _lift;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent = null;
        }
    }
}
