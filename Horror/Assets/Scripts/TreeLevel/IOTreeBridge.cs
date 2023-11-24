using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;
using UnityEngine.Playables;

public class IOTreeBridge : MonoBehaviour
{
    [SerializeField] PlayableDirector playableDirector;
    [SerializeField] float playbackSpeed = 1.0f;

    public void OnInteraction()
    {
        playableDirector.RebuildGraph(); // the graph must be created before getting the playable graph
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(playbackSpeed);
        playableDirector.Play();
    }
}
