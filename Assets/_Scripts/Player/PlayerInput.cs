using UnityEngine.InputSystem;
using UnityEngine;

namespace Voidwalker.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private Vector2 movementInput;
        private Vector3 mousePosition;

        private PlayerInputActions playerInputs;

        private void Awake()
        {
            playerInputs = new PlayerInputActions();
        }

        private void OnEnable() => playerInputs.Player.Enable();

        private void OnDisable() => playerInputs.Player.Disable();

        private void Update()
        {
            ReadMoveInput();
            ReadMousePosition();
        }

        private void ReadMoveInput()
        {
            movementInput = playerInputs.Player.Move.ReadValue<Vector2>();
        }

        private void ReadMousePosition()
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public Vector2 GetMovementInput() => movementInput;

        public Vector3 GetMousePosition()
        {
            return mousePosition;
        }
    }
}
