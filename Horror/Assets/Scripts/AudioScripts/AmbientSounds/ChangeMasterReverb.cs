using UnityEngine;

public class ChangeMasterReverb : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MasterReverb", 0.75f);
    }
}
