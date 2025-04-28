using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;

public class Scannable : MonoBehaviour
{
    public ScannableData Data;
    public UnityEvent OnScanned;
    public bool IsAudio = false;
    public bool WasScanned = false;

    [SerializeField, Tooltip("Audio event for talk sounds.")]
    public FMODUnity.EventReference talkEvent;

    [SerializeField] private Material overlayMaterial; // Materia� efektu "przed skanem"
    private Material overlayInstance;
    private Renderer meshRenderer;
    private Mesh copiedMesh;
    private GameObject copy;

    void Start()
    {
        meshRenderer = GetComponent<Renderer>();


        if (meshRenderer != null && overlayMaterial != null)
        {
            Mesh originalMesh = GetComponent<MeshFilter>().mesh;
            copiedMesh = Instantiate(originalMesh);

            // Stw�rz instancj� overlaya
            overlayInstance = Instantiate(overlayMaterial);

            copy = new GameObject("MeshCopy");
            copy.AddComponent<MeshFilter>().mesh = copiedMesh;
            copy.AddComponent<MeshRenderer>().materials = new Material[2] { overlayInstance, overlayInstance };
            copy.transform.SetParent(transform);
        }

        OnScanned.AddListener(OnScannedFunc);
    }

    void OnScannedFunc()
    {
        if (WasScanned) return; // �eby nie usuwa� kilka razy

        Destroy(copy);

        WasScanned = true;
    }
}
