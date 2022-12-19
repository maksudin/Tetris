using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Utils
{
    public class ControlsUtils : MonoBehaviour
    {
        public static void DisableInput()
        {
            FindObjectOfType<PlayerInput>().enabled = false;
        }

        public static void EnableInput()
        {
            FindObjectOfType<PlayerInput>().enabled = true;
        }
    }
}