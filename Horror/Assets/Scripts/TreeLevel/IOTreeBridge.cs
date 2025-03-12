using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Playables;

public class IOTreeBridge : MonoBehaviour
{
    [SerializeField] PlayableDirector playableDirector;
    [SerializeField] float playbackSpeed = 1.0f;
    [SerializeField] EventReference eventReference;

    public void OnInteraction()
    {
        playableDirector.RebuildGraph();
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(playbackSpeed);
        playableDirector.Play();
        RuntimeManager.PlayOneShot(eventReference, transform.position);
    }
}
