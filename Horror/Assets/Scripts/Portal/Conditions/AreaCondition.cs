using UnityEngine;

public class AreaCondition : JustCondition
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            value = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            value = false;
        }
    }

    public override void ReCheckTrigger()
    {
        if (player != null)
        {
            value = collider.bounds.Contains(player.transform.position);
        }
    }
}
