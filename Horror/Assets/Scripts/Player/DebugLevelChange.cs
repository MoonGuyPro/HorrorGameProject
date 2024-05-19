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
                case 'O':
                    DebugChange("Labs");
                    break;
                case 'P':
                    DebugChange("TreeLevel");
                    break;
                case '{':
                    DebugChange("Labs 1");
                    break;
                case '}':
                    DebugChange("Labyrinth_New");
                    break;
                case '|':
                    DebugChange("Caves");
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