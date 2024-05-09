using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class Portal : MonoBehaviour
{
    [Header("Main Settings")]
    public Portal linkedPortal;
    public MeshRenderer screen;
    public int recursionLimit = 5;
    public RenderReplacementShaderToTexture renderReplacement;

    [Header("Advanced Settings")]
    public float nearClipOffset = 0.05f;
    public float nearClipLimit = 0.2f;
    public List<Portal> isSeenByPortals;
    [SerializeField] List<JustCondition> otherConditions;
    [SerializeField] bool linkedPortalIsSeen;

    // Private variables
    RenderTexture viewTexture1;
    RenderTexture viewTexture2;
    RenderTexture depthTexture1;
    RenderTexture depthTexture2;
    Camera playerCam;
    Camera renderCam;
    Camera portalCam;
    Camera noShaderCam;
    Camera normalCam;
    List<PortalTraveller> trackedTravellers;
    MeshFilter screenMeshFilter;

    void Awake()
    {
        playerCam = Camera.main;
        portalCam = GetComponentInChildren<Camera>();
        portalCam.enabled = false;
        portalCam.depthTextureMode = DepthTextureMode.Depth;
        trackedTravellers = new List<PortalTraveller>();
        screenMeshFilter = screen.GetComponent<MeshFilter>();
        screen.material.SetInt("displayMask", 1);
        if(isSeenByPortals.Count == 0) {
            isSeenByPortals = new List<Portal>();
        } 
        if(otherConditions.Count == 0) {
            otherConditions = new List<JustCondition>();
        }
        renderCam = null;
    }

    private void Start()
    {
        for (int i = 0; i < portalCam.gameObject.transform.childCount; i++)
        {
            if (portalCam.gameObject.transform.GetChild(i).name == "Camera_CameraNormalsTexture")
            {
                normalCam = portalCam.gameObject.transform.GetChild(i).GetComponent<Camera>();
                normalCam.enabled = false;
            }
            else
            {
                noShaderCam = portalCam.gameObject.transform.GetChild(i).GetComponent<Camera>();
                noShaderCam.enabled = false;
                noShaderCam.depthTextureMode = DepthTextureMode.Depth;
            }
        }
        if (noShaderCam == null || normalCam == null || portalCam == null)
        {
            Debug.LogError("Camera not set correctly in camera " + gameObject.name);
            gameObject.SetActive(false);
        }
        if (renderReplacement == null)
        {
            Debug.LogError("Render replacment not set correctly in camera " + gameObject.name);
            gameObject.SetActive(false);
        }
        renderCam = Camera.main;
    }

    void LateUpdate()
    {
        HandleTravellers();
    }

    void HandleTravellers()
    {

        for (int i = 0; i < trackedTravellers.Count; i++)
        {
            PortalTraveller traveller = trackedTravellers[i];
            Transform travellerT = traveller.transform;
            var m = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerT.localToWorldMatrix;

            Vector3 offsetFromPortal = travellerT.position - transform.position;
            int portalSide = System.Math.Sign(Vector3.Dot(offsetFromPortal, transform.forward));
            int portalSideOld = System.Math.Sign(Vector3.Dot(traveller.previousOffsetFromPortal, transform.forward));
            // Teleport the traveller if it has crossed from one side of the portal to the other
            if (portalSide != portalSideOld)
            {
                var positionOld = travellerT.position;
                var rotOld = travellerT.rotation;
                traveller.Teleport(transform, linkedPortal.transform, m.GetColumn(3), m.rotation);
                traveller.graphicsClone.transform.SetPositionAndRotation(positionOld, rotOld);
                // Can't rely on OnTriggerEnter/Exit to be called next frame since it depends on when FixedUpdate runs
                linkedPortal.OnTravellerEnterPortal(traveller);
                trackedTravellers.RemoveAt(i);
                i--;
            }
            else
            {
                traveller.graphicsClone.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
                //UpdateSliceParams (traveller);
                traveller.previousOffsetFromPortal = offsetFromPortal;
            }
        }
    }

    // Called before any portal cameras are rendered for the current frame
    public void PrePortalRender()
    {
        foreach (var traveller in trackedTravellers)
        {
            UpdateSliceParams(traveller);
        }
    }

    // Manually render the camera attached to this portal
    // Called after PrePortalRender, and before PostPortalRender
    public void Render()
    {
        // Skip rendering the view from this portal if player is not looking at the linked portal
        if (!CameraUtility.VisibleFromCamera(linkedPortal.screen, playerCam) 
            || (linkedPortal.otherConditions.All(c => c.value) &&
                linkedPortal.isSeenByPortals.Any(p => p.linkedPortalIsSeen &&
                 CameraUtility.VisibleFromCamera(linkedPortal.screen, p.noShaderCam)))
                 )
        {
            var portalLooking = linkedPortal.isSeenByPortals.FirstOrDefault(p => p.linkedPortalIsSeen &&
                        CameraUtility.VisibleFromCamera(linkedPortal.screen, p.noShaderCam));

            if (portalLooking == null)
            {
                linkedPortalIsSeen = false;
                return;
            }
            renderCam = portalLooking.portalCam;
        }
        else
        {
            renderCam = Camera.main;
            Debug.Log("Hello from "+gameObject.name);
        }
        linkedPortalIsSeen = true;

        CreateViewTexture();

        var localToWorldMatrix = renderCam.transform.localToWorldMatrix;
        var renderPositions = new Vector3[recursionLimit];
        var renderRotations = new Quaternion[recursionLimit];

        int startIndex = 0;
        portalCam.projectionMatrix = renderCam.projectionMatrix;
        normalCam.projectionMatrix = renderCam.projectionMatrix;
        noShaderCam.projectionMatrix = renderCam.projectionMatrix;
        for (int i = 0; i < recursionLimit; i++)
        {
            if (i > 0)
            {
                // No need for recursive rendering if linked portal is not visible through this portal
                if (!CameraUtility.BoundsOverlap(screenMeshFilter, linkedPortal.screenMeshFilter, portalCam))
                {
                    break;
                }
            }
            localToWorldMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * localToWorldMatrix;
            int renderOrderIndex = recursionLimit - i - 1;
            renderPositions[renderOrderIndex] = localToWorldMatrix.GetColumn(3);
            renderRotations[renderOrderIndex] = localToWorldMatrix.rotation;

            portalCam.transform.SetPositionAndRotation(renderPositions[renderOrderIndex], renderRotations[renderOrderIndex]);
            startIndex = renderOrderIndex;
        }

        // Hide screen so that camera can see through portal
        screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        linkedPortal.screen.material.SetInt("displayMask", 0);

        for (int i = startIndex; i < recursionLimit; i++)
        {
            portalCam.transform.SetPositionAndRotation(renderPositions[i], renderRotations[i]);
            SetNearClipPlane();
            HandleClipping();

            renderReplacement.RenderNormals();

            RenderDepth();
            portalCam.Render();
            noShaderCam.Render();

            RenderColor();
            portalCam.Render();
            noShaderCam.Render();

            if (i == startIndex)
            {
                linkedPortal.screen.material.SetInt("displayMask", 1);
            }
        }

        // Unhide objects hidden at start of render
        screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    void HandleClipping()
    {
        // There are two main graphical issues when slicing travellers
        // 1. Tiny sliver of mesh drawn on backside of portal
        //    Ideally the oblique clip plane would sort this out, but even with 0 offset, tiny sliver still visible
        // 2. Tiny seam between the sliced mesh, and the rest of the model drawn onto the portal screen
        // This function tries to address these issues by modifying the slice parameters when rendering the view from the portal
        // Would be great if this could be fixed more elegantly, but this is the best I can figure out for now
        const float hideDst = -1000;
        const float showDst = 1000;
        float screenThickness = linkedPortal.ProtectScreenFromClipping(portalCam.transform.position);

        foreach (var traveller in trackedTravellers)
        {
            if (SameSideOfPortal(traveller.transform.position, portalCamPos))
            {
                // Addresses issue 1
                traveller.SetSliceOffsetDst(hideDst, false);
            }
            else
            {
                // Addresses issue 2
                traveller.SetSliceOffsetDst(showDst, false);
            }

            // Ensure clone is properly sliced, in case it's visible through this portal:
            int cloneSideOfLinkedPortal = -SideOfPortal(traveller.transform.position);
            bool camSameSideAsClone = linkedPortal.SideOfPortal(portalCamPos) == cloneSideOfLinkedPortal;
            if (camSameSideAsClone)
            {
                traveller.SetSliceOffsetDst(screenThickness, true);
            }
            else
            {
                traveller.SetSliceOffsetDst(-screenThickness, true);
            }
        }

        var offsetFromPortalToCam = portalCamPos - transform.position;
        foreach (var linkedTraveller in linkedPortal.trackedTravellers)
        {
            var travellerPos = linkedTraveller.graphicsObject.transform.position;
            var clonePos = linkedTraveller.graphicsClone.transform.position;
            // Handle clone of linked portal coming through this portal:
            bool cloneOnSameSideAsCam = linkedPortal.SideOfPortal(travellerPos) != SideOfPortal(portalCamPos);
            if (cloneOnSameSideAsCam)
            {
                // Addresses issue 1
                linkedTraveller.SetSliceOffsetDst(hideDst, true);
            }
            else
            {
                // Addresses issue 2
                linkedTraveller.SetSliceOffsetDst(showDst, true);
            }

            // Ensure traveller of linked portal is properly sliced, in case it's visible through this portal:
            bool camSameSideAsTraveller = linkedPortal.SameSideOfPortal(linkedTraveller.transform.position, portalCamPos);
            if (camSameSideAsTraveller)
            {
                linkedTraveller.SetSliceOffsetDst(screenThickness, false);
            }
            else
            {
                linkedTraveller.SetSliceOffsetDst(-screenThickness, false);
            }
        }
    }

    // Called once all portals have been rendered, but before the player camera renders
    public void PostPortalRender()
    {
        foreach (var traveller in trackedTravellers)
        {
            UpdateSliceParams(traveller);
        }
        ProtectScreenFromClipping(renderCam.transform.position);
    }

    void CreateViewTexture()
    {
        if (viewTexture1 == null || viewTexture1.width != Screen.width || viewTexture1.height != Screen.height ||
            viewTexture2 == null || viewTexture2.width != Screen.width || viewTexture2.height != Screen.height ||
            depthTexture1 == null || depthTexture1.width != Screen.width || depthTexture1.height != Screen.height ||
            depthTexture2 == null || depthTexture2.width != Screen.width || depthTexture2.height != Screen.height)
        {
            if (viewTexture1 != null)
            {
                viewTexture1.Release();
            }
            if (viewTexture2 != null)
            {
                viewTexture2.Release();
            }
            if (depthTexture1 != null)
            {
                depthTexture1.Release();
            }
            if (depthTexture2 != null)
            {
                depthTexture2.Release();
            }


            viewTexture1 = new RenderTexture(Screen.width, Screen.height, 16);
            viewTexture2 = new RenderTexture(Screen.width, Screen.height, 16);

            depthTexture1 = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.Depth);
            depthTexture2 = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.Depth);
        }
    }

    private void RenderDepth()
    {
        portalCam.targetTexture = depthTexture1;
        noShaderCam.targetTexture = depthTexture2;

        portalCam.SetTargetBuffers(depthTexture1.colorBuffer, depthTexture1.depthBuffer);
        noShaderCam.SetTargetBuffers(depthTexture2.colorBuffer, depthTexture2.depthBuffer);

        linkedPortal.screen.material.SetTexture("_DepthTex1", depthTexture1);
        linkedPortal.screen.material.SetTexture("_DepthTex2", depthTexture2);
    }

    private void RenderColor()
    {
        portalCam.targetTexture = viewTexture1;
        noShaderCam.targetTexture = viewTexture2;

        linkedPortal.screen.material.SetTexture("_MainTex1", viewTexture1);
        linkedPortal.screen.material.SetTexture("_MainTex2", viewTexture2);
    }


    // Sets the thickness of the portal screen so as not to clip with camera near plane when player goes through
    float ProtectScreenFromClipping(Vector3 viewPoint)
    {
        float halfHeight = renderCam.nearClipPlane;
        halfHeight *= Mathf.Tan(renderCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * renderCam.aspect;
        float dstToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, renderCam.nearClipPlane).magnitude;
        float screenThickness = dstToNearClipPlaneCorner;

        Transform screenT = screen.transform;
        bool camFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - viewPoint) > 0;
        screenT.localScale = new Vector3(screenT.localScale.x, screenT.localScale.y, screenThickness);
        screenT.localPosition = Vector3.forward * screenThickness * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);
        return screenThickness;
    }

    void UpdateSliceParams(PortalTraveller traveller)
    {
        // Calculate slice normal
        int side = SideOfPortal(traveller.transform.position);
        Vector3 sliceNormal = transform.forward * -side;
        Vector3 cloneSliceNormal = linkedPortal.transform.forward * side;

        // Calculate slice centre
        Vector3 slicePos = transform.position;
        Vector3 cloneSlicePos = linkedPortal.transform.position;

        // Adjust slice offset so that when player standing on other side of portal to the object, the slice doesn't clip through
        float sliceOffsetDst = 0;
        float cloneSliceOffsetDst = 0;
        float screenThickness = screen.transform.localScale.z;

        bool playerSameSideAsTraveller = SameSideOfPortal(renderCam.transform.position, traveller.transform.position);
        if (!playerSameSideAsTraveller)
        {
            sliceOffsetDst = -screenThickness;
        }
        bool playerSameSideAsCloneAppearing = side != linkedPortal.SideOfPortal(renderCam.transform.position);
        if (!playerSameSideAsCloneAppearing)
        {
            cloneSliceOffsetDst = -screenThickness;
        }

        // Apply parameters
        for (int i = 0; i < traveller.originalMaterials.Length; i++)
        {
            traveller.originalMaterials[i].SetVector("sliceCentre", slicePos);
            traveller.originalMaterials[i].SetVector("sliceNormal", sliceNormal);
            traveller.originalMaterials[i].SetFloat("sliceOffsetDst", sliceOffsetDst);

            traveller.cloneMaterials[i].SetVector("sliceCentre", cloneSlicePos);
            traveller.cloneMaterials[i].SetVector("sliceNormal", cloneSliceNormal);
            traveller.cloneMaterials[i].SetFloat("sliceOffsetDst", cloneSliceOffsetDst);
        }

    }

    // Use custom projection matrix to align portal camera's near clip plane with the surface of the portal
    // Note that this affects precision of the depth buffer, which can cause issues with effects like screenspace AO
    void SetNearClipPlane()
    {
        // Learning resource:
        // http://www.terathon.com/lengyel/Lengyel-Oblique.pdf
        Transform clipPlane = transform;
        int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, transform.position - portalCam.transform.position));

        Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
        Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
        float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal) + nearClipOffset;

        // Don't use oblique clip plane if very close to portal as it seems this can cause some visual artifacts
        if (Mathf.Abs(camSpaceDst) > nearClipLimit)
        {
            Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

            // Update projection based on new clip plane
            // Calculate matrix with player cam so that player camera settings (fov, etc) are used
            portalCam.projectionMatrix = renderCam.CalculateObliqueMatrix(clipPlaneCameraSpace);
            normalCam.projectionMatrix = portalCam.projectionMatrix;
            noShaderCam.projectionMatrix = portalCam.projectionMatrix;
        }
        else
        {
            portalCam.projectionMatrix = renderCam.projectionMatrix;
            normalCam.projectionMatrix = portalCam.projectionMatrix;
            noShaderCam.projectionMatrix = portalCam.projectionMatrix;
        }
    }

    void OnTravellerEnterPortal(PortalTraveller traveller)
    {
        if (!trackedTravellers.Contains(traveller))
        {
            traveller.EnterPortalThreshold();
            traveller.previousOffsetFromPortal = traveller.transform.position - transform.position;
            trackedTravellers.Add(traveller);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var traveller = other.GetComponent<PortalTraveller>();
        if (traveller)
        {
            OnTravellerEnterPortal(traveller);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var traveller = other.GetComponent<PortalTraveller>();
        if (traveller && trackedTravellers.Contains(traveller))
        {
            traveller.ExitPortalThreshold();
            trackedTravellers.Remove(traveller);
        }
    }

    /*
     ** Some helper/convenience stuff:
     */

    int SideOfPortal(Vector3 pos)
    {
        return System.Math.Sign(Vector3.Dot(pos - transform.position, transform.forward));
    }

    bool SameSideOfPortal(Vector3 posA, Vector3 posB)
    {
        return SideOfPortal(posA) == SideOfPortal(posB);
    }

    Vector3 portalCamPos
    {
        get
        {
            return portalCam.transform.position;
        }
    }

    void OnValidate()
    {
        if (linkedPortal != null)
        {
            linkedPortal.linkedPortal = this;
        }
    }

}