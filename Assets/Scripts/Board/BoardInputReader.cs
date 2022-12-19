using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Board
{
    public class BoardInputReader : MonoBehaviour
    {
        [SerializeField] private Board _board;

        private void Awake()
        {
            ControlsUtils.DisableInput();
        }

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
                _board.RotateTetromino();
        }

        public void OnCounterRotation(InputAction.CallbackContext context)
        {
            if (context.performed)
                _board.RotateTetromino(isClockwise: false);
        }

        public void OnHardDrop(InputAction.CallbackContext context)
        {
            if (context.performed)
                _board.HardDrop();
        }

        public void OnRestart(InputAction.CallbackContext context)
        {
            if (context.performed)
                _board.Restart();
        }
    }
}