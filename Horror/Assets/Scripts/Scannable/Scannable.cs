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

    public Material overlayMaterial; // Materia³ efektu "przed skanem"
    private GameObject copy;
    private Mesh originalMesh;

    void Awake()
    {
        if(GetComponent<MeshFilter>() != null && GetComponent<MeshFilter>().sharedMesh != null)
            originalMesh = GetComponent<MeshFilter>().sharedMesh;
    }

    void Start()
    {
        Renderer meshRenderer = GetComponent<Renderer>();


        if (meshRenderer != null && overlayMaterial != null)
        {
            Mesh copiedMesh = Instantiate(originalMesh);

            // Stwórz instancjê overlaya
            Material overlayInstance = Instantiate(overlayMaterial);

            copy = new GameObject("MeshCopy");
            copy.AddComponent<MeshFilter>().mesh = copiedMesh;
            Material[] overlays = new Material[GetComponent<MeshRenderer>().materials.Length];
            for (int i = 0; i < overlays.Length; i++)
            {
                overlays[i] = overlayInstance;
            }
            copy.AddComponent<MeshRenderer>().materials = overlays;
            copy.GetComponent<MeshRenderer>().receiveShadows = false;
            copy.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
            copy.transform.SetParent(transform, true);
            copy.transform.localScale = Vector3.one;
            copy.transform.localPosition = Vector3.zero;
            copy.transform.localRotation = Quaternion.identity;
            copy.isStatic = true;
        }

        OnScanned.AddListener(OnScannedFunc);
    }

    void OnScannedFunc()
    {
        if (WasScanned) return; // ¿eby nie usuwaæ kilka razy

        Destroy(copy);

        WasScanned = true;
    }
}
