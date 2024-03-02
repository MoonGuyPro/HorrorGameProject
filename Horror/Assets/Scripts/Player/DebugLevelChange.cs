using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class DebugLevelChange : MonoBehaviour
    {

        void OnEnable ()
        {
            Keyboard.current.onTextInput += OnTextInput;
        }

        void OnDisable ()
        {
            Keyboard.current.onTextInput -= OnTextInput;
        }

        void OnTextInput(char ch)
        {
            switch (ch)
            {
                case 'i':
                    DebugChange("TreeLevel");
                    break;
                case 'o':
                    DebugChange("TreeLevel2");
                    break;
                case '\\':
                    DebugChange("Caves");
                    break;
                case '[':
                    DebugChange("Labyrinth");
                    break;
                case ']':
                    DebugChange("Lights");
                    break;
                case 'l':
                    DebugChange("Walls");
                    break;
                case ';':
                    DebugChange("EasyMechanics");
                    break;
                
            }
        }

        private void DebugChange(string level)
        {
            Debug.Log("Debug level change to: " + level);
            SceneManager.LoadScene(level);
        }
    }
}