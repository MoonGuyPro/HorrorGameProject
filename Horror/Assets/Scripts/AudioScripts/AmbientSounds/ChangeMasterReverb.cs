using UnityEngine;

public class ChangeMasterReverb : MonoBehaviour
{
    // Start is called before the first frame update

    public float reverbValue = 0.75f;
    
    void Start()
    {
        if (reverbValue > 1.0f)
        {
            reverbValue = 1.0f;
        }
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MasterReverb", reverbValue);
    }
}
