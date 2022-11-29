using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Board
{
    public class BoardInputReader : MonoBehaviour
    {
        [SerializeField] private Board _board;

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var direction = context.ReadValue<Vector2>();
                _board.Move(direction);
            }
        }

        public void OnRotation(InputAction.CallbackContext context)
        {
            if (context.performed)
                _board.RotateClockWise();
        }

        public void OnRush(InputAction.CallbackContext context)
        {
            if (context.performed)
                _board.Rush();
        }
    }
}