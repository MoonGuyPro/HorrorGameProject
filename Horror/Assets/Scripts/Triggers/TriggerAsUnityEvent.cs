using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerAsUnityEvent : MonoBehaviour
{
    [SerializeField]
    private bool oneshot = true;
    public bool Oneshot { get => oneshot; }

    public UnityEvent OnTrigger;

    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            OnTrigger.Invoke();

            if (oneshot)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void OnDrawGizmos()
    {
        // Cache the original Gizmos matrix
        Matrix4x4 originalMatrix = Gizmos.matrix;

        // Set gizmo color
        Gizmos.color = new Color(1, 0, 0, 0.3f);

        // Loop through all BoxColliders
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        foreach (var col in colliders)
        {
            // Build the transformation matrix for this collider
            Gizmos.matrix = Matrix4x4.TRS(
                transform.position + col.center,
                transform.rotation,
                transform.lossyScale
            );

            // Draw the cube in local space (centered at origin)
            Gizmos.DrawCube(Vector3.zero, col.size);
        }

        // Restore original matrix
        Gizmos.matrix = originalMatrix;
    }
}
