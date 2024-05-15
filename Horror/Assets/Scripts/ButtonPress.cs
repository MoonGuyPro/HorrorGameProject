using DG.Tweening;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public void OnInteraction()
    {
        DOTween.To(
            () => 0.0f, 
            x => transform.localPosition = new Vector3(0, -Mathf.Sin(x) * 0.03f, 0), 
            Mathf.PI, 
            1.0f
        );
    }
}
