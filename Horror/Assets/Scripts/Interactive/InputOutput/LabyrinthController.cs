using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthController : MonoBehaviour
{
    [Tooltip("Add reference to gameobjects that contain labyrinth blocks as children.")]
    [SerializeField] List<GameObject> labyrinths;
    private List<Animator> animators = new List<Animator>();

    void Start()
    {
        labyrinths.ForEach(
            labyrinth => animators.AddRange(labyrinth.GetComponentsInChildren<Animator>())
            );
    }

    public void OnSolved()
    {
        animators.ForEach(a => a.SetBool("active", true));
    }

}
