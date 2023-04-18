using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class DebugLevelChange : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                DebugChange("TreeLevel");
            if (Input.GetKeyDown(KeyCode.O))
                DebugChange("TreeLevel2");
            if (Input.GetKeyDown(KeyCode.P))
                DebugChange("Caves");
            if (Input.GetKeyDown(KeyCode.LeftBracket))
                DebugChange("Labyrinth");
            if (Input.GetKeyDown(KeyCode.RightBracket))
                DebugChange("Lights");
            if (Input.GetKeyDown(KeyCode.L))
                DebugChange("Walls");
            if (Input.GetKeyDown(KeyCode.Semicolon))
                DebugChange("EasyMechanics");
        }

        private void DebugChange(string level)
        {
            Debug.Log("Debug level change to: " + level);
            SceneManager.LoadScene(level);
        }
    }
}