using UnityEngine;

public class MainCamera : MonoBehaviour {

    public RenderReplacementShaderToTexture renderReplacement;
    Portal[] portals;

    public void FindPortals() {
        portals = FindObjectsOfType<Portal> ();
    }

    void Awake () {
        FindPortals();
    }

    void OnPreCull () {

        for (int i = 0; i < portals.Length; i++) {
            portals[i].PrePortalRender ();
        }
        
        for (int i = 0; i < portals.Length; i++) {
            portals[i].Render ();
        }
        

        for (int i = 0; i < portals.Length; i++) {
            portals[i].PostPortalRender ();
        }

        if(renderReplacement != null)
        {
            renderReplacement.RenderNormals();
        }
    }
}