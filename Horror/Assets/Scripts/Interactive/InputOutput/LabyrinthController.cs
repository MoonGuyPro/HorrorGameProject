using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthController : MonoBehaviour
{
    [Tooltip("Add reference to gameobjects that contain labyrinth blocks as children.")]
    [SerializeField] List<GameObject> labyrinths;

    // Used both for block and platforms, but also hints
    private List<Animator> animators = new List<Animator>();
    private List<LabyPlatform> labyPlatforms = new List<LabyPlatform>();

    void Start()
    {
        labyrinths.ForEach(
            labyrinth => 
            {
                animators.AddRange(labyrinth.GetComponentsInChildren<Animator>());
                labyPlatforms.AddRange(labyrinth.GetComponentsInChildren<LabyPlatform>());
            }
        );
    }

    public void OnSolved()
    {
        animators.ForEach(a => a.SetBool("active", true));
        labyPlatforms.ForEach(p => p.DisableScaling());
    }
}
