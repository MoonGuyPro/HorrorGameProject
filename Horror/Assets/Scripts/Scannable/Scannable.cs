using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        var smr = GetComponent<SkinnedMeshRenderer>();
        var mf = GetComponent<MeshFilter>();

        gameObject.isStatic = false;

        if (smr != null && smr.sharedMesh != null)
        {
            smr.BakeMesh(originalMesh);
        }
        else if (mf != null && mf.sharedMesh != null)
        {
            originalMesh = GetComponent<MeshFilter>().mesh;
            originalMesh.vertices = mf.mesh.vertices;
            originalMesh.normals = mf.mesh.normals;
            originalMesh.uv = mf.mesh.uv;
            originalMesh.triangles = mf.mesh.triangles;
            originalMesh.tangents = mf.mesh.tangents;
        }
    }

    void Start()
    {
        gameObject.isStatic = true;
        Renderer meshRenderer = GetComponent<Renderer>();

        if (meshRenderer != null && overlayMaterial != null)
        {
            // Stwórz instancjê overlaya
            Material overlayInstance = Instantiate(overlayMaterial);

            copy = new GameObject("MeshCopy");
            copy.AddComponent<MeshFilter>().mesh = originalMesh;
            Material[] overlays = new Material[GetComponent<MeshRenderer>().materials.Length];
            for (int i = 0; i < overlays.Length; i++)
            {
                overlays[i] = overlayInstance;
            }
            copy.AddComponent<MeshRenderer>().materials = overlays;
            //copy.GetComponent<MeshRenderer>().receiveShadows = false;
            //copy.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
            copy.transform.SetParent(transform, true);
            copy.transform.localScale = Vector3.one;
            copy.transform.localPosition = Vector3.zero;
            copy.transform.localRotation = Quaternion.identity;
            //copy.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            copy.isStatic = true;
            //copy.layer = 9;
            //gameObject.layer = 9;
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
