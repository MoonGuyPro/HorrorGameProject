using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IOTreeBridge : MonoBehaviour
{
    [SerializeField] PlayableDirector playableDirector;
    [SerializeField] float playbackSpeed = 1.0f;

    public void OnInteraction()
    {
        playableDirector.RebuildGraph();
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(playbackSpeed);
        playableDirector.Play();
    }
}
